using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using LibGit2Sharp;
using Page_Navigation_App.ViewModel;

namespace Page_Navigation_App
{
    public partial class GameView : Window
    {
        private bool isInstalled = false;
        public string ShortDescription { get; set; }
        public string GameDescription { get; set; }
        public string GameGenre { get; set; }
       
        public string GameTitle { get; set; }
        public string ThumbnailImageSource { get; set; }
        private string targetDirectory;
        
        private void InitializeDirectories(string idda)
        {
            
            targetDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), idda);
          
        }


        public string repositoryUrl { get; set; } 
        
        public bool isBusy = false;
        private bool isCloning = false;
        public bool IsCloning
        {
            get { return isCloning; }
            private set
            {
                isCloning = value;
                // Notify the UI or perform other actions when the value changes.
            }
        }

        public GameView(GameViewModel viewModel)
        {
          
            DataContext = viewModel;
            InitializeComponent();
            InitializeDirectories(viewModel.GameTitle);
            repositoryUrl = viewModel.repositoryUrl;
            if (IsApplicationInstalled())
            {
                myButton.Content = "Play";
                if (IsUpdateAvailable())
                {
                    myButton.Content = "Update";
                }
            }
           
        }

        private async void PlayOrInstall(object sender, RoutedEventArgs e)
        {
            if (isBusy||IsCloning) return;
            isBusy = true;
            var viewModel = DataContext as GameViewModel;
            if (viewModel.Price == "Үнэгүй"|| AppData.CurrentUser.premium)
            {
                try
                {
                    if (isInstalled || IsApplicationInstalled())
                    {
                        if (IsUpdateAvailable())
                        {
                            myButton.Content = "Updating...";
                            await Task.Run(() => HandleUpdate());
                        }

                        Play();
                    }
                    else
                    {
                        IsCloning = true;
                        myButton.Content = "Installing..."; // Set to "Installing..." before starting the clone process

                        await Task.Run(() =>
                        {
                            if (!Directory.Exists(targetDirectory))
                            {
                                Directory.CreateDirectory(targetDirectory);
                            }

                            if (Repository.IsValid(targetDirectory))
                            {
                                Dispatcher.Invoke(() => MessageBox.Show("Repository already exists."));
                                isInstalled = true;
                            }
                            else
                            {
                                // Clone the repository using the correct URL
                                Repository.Clone(repositoryUrl, targetDirectory);
                                IsCloning = false;

                                isInstalled = true;
                            }
                        });

                        myButton.Content = "Play"; // Update to "Play" after the cloning is done
                    }
                }
                catch (Exception ex)
                {
                    IsCloning = false;
                    Dispatcher.Invoke(() => MessageBox.Show("Error: " + ex.Message));
                }
                finally
                {
                    IsCloning = false;
                    isBusy = false; // Ensure isBusy is reset correctly
                    Dispatcher.Invoke(() => myButton.Content = isInstalled ? "Play" : "Install");
                }
            }
            else
            {
                MessageBox.Show("Үйлчлгээний эрхээ сунга нуу");
            }
       
        }




        private bool IsApplicationInstalled()
        {
            return Directory.Exists(targetDirectory);
        }

        private void Play()
        {
            var viewModel = DataContext as GameViewModel;

            if (viewModel != null)
            {
                string gameExecutablePath = Path.Combine(targetDirectory, viewModel.GameTitle + ".exe");

                if (File.Exists(gameExecutablePath))
                {
                    try
                    {
                        // Example of user data
                        string userDataArgument = $"-premium {AppData.CurrentUser.premium.ToString().ToLower()} -price \"{viewModel.Price}\"";


                        ProcessStartInfo startInfo = new ProcessStartInfo(gameExecutablePath)
                        {
                            WorkingDirectory = Path.GetDirectoryName(gameExecutablePath),
                            UseShellExecute = true,
                            Arguments = userDataArgument // Passing user data as arguments
                        };
                        Process gameProcess = Process.Start(startInfo);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to start the game: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Game executable not found.");
                }
            }
            else
            {
                MessageBox.Show("Game data is not available.");
            }
        }






        private bool IsUpdateAvailable()
        {
            using (var repo = new Repository(targetDirectory))
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = $"fetch origin",
                    WorkingDirectory = targetDirectory,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        // Fetch was successful, now you can compare the local and remote branches as before.
                        var localBranch = repo.Branches["main"]; // Change 'master' to your default branch name
                        var remoteBranch = repo.Branches[$"origin/{localBranch.FriendlyName}"];
                        Debug.WriteLine(localBranch.FriendlyName);
                        
                        return localBranch.Tip.Sha != remoteBranch.Tip.Sha;
                    }
                    else
                    {
                        // Handle fetch error here if needed
                        // You can check process.StandardOutput and process.StandardError for error messages.
                        return false;
                    }
                }
            }
        }


        private void HandleUpdate()
        {
           // myButton.Content = "Updating";

            try
            {
                using (var repo = new Repository(targetDirectory))
                {
                    var remote = repo.Network.Remotes["origin"];
                    var options = new PullOptions
                    {
                        FetchOptions = new FetchOptions
                        {
                            CredentialsProvider = (url, usernameFromUrl, types) =>
                                new UsernamePasswordCredentials
                                {
                                    Username = "your_username", // Replace with your Git username
                                    Password = "your_password"  // Replace with your Git password or token
                                }
                        }
                    };

                    // Perform a git pull to update the local repository
                    Commands.Pull(repo, new Signature("Your Name", "your_email@example.com", DateTimeOffset.Now), options);
                    isBusy = false;
                    myButton.Content = "Play";
                   // Handle the update completion logic here
                   MessageBox.Show("Update completed successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during update: " + ex.Message);
            }
        }

    }
}
