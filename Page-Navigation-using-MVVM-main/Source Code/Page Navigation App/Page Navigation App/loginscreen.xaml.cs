using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using AutoUpdaterDotNET;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Page_Navigation_App;
using SharpCompress.Archives;
using SharpCompress.Common;

namespace Page_Navigation_App
{
    public partial class loginscreen : Window
    {
        public loginscreen()
        {
            InitializeComponent();
            UsernameTextBox.KeyDown += OnKeyDownHandler;
            PasswordBox.KeyDown += OnKeyDownHandler;

        }
        private bool isOffline = false;
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = UsernameTextBox.Text;
                string password = PasswordBox.Password;
                bool rememberMe = RememberMeCheckBox.IsChecked ?? false;

                ApiResponse response = await CallApiAsync(username, password);

                if (response.IsSuccess)
                {
                    AppData.CurrentUser = response.UserData;
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    Close();

                    if (rememberMe)
                    {
                        SecureStorage.StoreCredentials(username, password);
                    }
                }
                else
                {
                    if (isOffline)
                    {
                        MessageBox.Show("Failed to connect to the server.");
                    }
                    else
                    {
                        MessageBox.Show("Incorrect username or password.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async Task<ApiResponse> CallApiAsync(string username, string password)
        {
            string baseUrl = "https://naadgai.mn/authenticateUser.php";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = $"{baseUrl}?username={HttpUtility.UrlEncode(username)}&password={HttpUtility.UrlEncode(password)}";

                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        UserData userData = ParseUserData(responseBody);
                        return new ApiResponse { IsSuccess = true, UserData = userData };
                    }
                    else
                    {
                        return new ApiResponse { IsSuccess = false, UserData = null };
                    }
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine($"Error: {ex.Message}");
                    return new ApiResponse { IsSuccess = false, UserData = null };
                }
            }
        }

        private UserData ParseUserData(string responseBody)
        {
            // Parse the JSON array string into a JArray
            JArray jsonArray = JArray.Parse(responseBody);

            // Ensure the array is not empty
            if (jsonArray.Count > 0)
            {
                // Get the first object from the array
                JObject jsonObject = jsonArray[0] as JObject;

                // Deserialize the JSON object into a UserData object
                UserData userData = jsonObject.ToObject<UserData>();
                return userData;
            }
            else
            {
                // Handle the case where the JSON array is empty
                throw new Exception("JSON array is empty.");
            }
        }



        private string GetDownloadLink(string xmlContent)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlContent);

                XmlNode urlNode = xmlDoc.SelectSingleNode("//url");
                return urlNode?.InnerText;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error extracting download link from XML: {ex.Message}");
            }
        }
        private async void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args.Error == null)
            {
                if (args.IsUpdateAvailable)
                {
                    MessageBoxResult dialogResult;
                    if (args.Mandatory.Value)
                    {
                        dialogResult = MessageBox.Show(
                            $@"New version {args.CurrentVersion} is available. You are currently using version {args.InstalledVersion}. This is a mandatory update. Click OK to update.",
                            @"Update Available", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        dialogResult = MessageBox.Show(
                            $@"New version {args.CurrentVersion} is available. You are currently using version {args.InstalledVersion}. Do you want to update?",
                            @"Update Available", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    }

                    if (dialogResult == MessageBoxResult.Yes || dialogResult == MessageBoxResult.OK)
                    {
                        try
                        {
                            // Display the updating screen
                            var updatingScreen = new UpdatingScreen();
                            updatingScreen.Show();

                            // Get the download link from the XML
                            string xmlContent = GetXmlContent("https://www.baaduu.me/version.xml");
                            string downloadLink = GetDownloadLink(xmlContent);

                            // Download and process the file
                            if (!string.IsNullOrEmpty(downloadLink))
                            {
                                await DownloadRarFile(downloadLink);

                                // Close the updating screen
                                updatingScreen.Close();
                            }
                            else
                            {
                                MessageBox.Show("Invalid XML format. Unable to get download link.");
                            }
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {

                }
            }
            else
            {
                if (args.Error is WebException)
                {
                    MessageBox.Show(
                        @"Failed to connect to the update server. Please check your internet connection and try again later.",
                        @"Update Check Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(args.Error.Message, args.Error.GetType().ToString(), MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }
        private string GetXmlContent(string xmlUrl)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    return client.DownloadString(xmlUrl);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error downloading XML content: {ex.Message}");
            }
        }
        private async Task DownloadRarFile(string url)
        {
            try
            {
                string downloadPath = @"C:\Downloads\YourApplicationArchive.rar"; // Set your desired download path
                string extractPath = @"C:\Downloads\"; // Set your desired extraction path
                // Ensure the directory exists, create it if it doesn't
                string directory = Path.GetDirectoryName(downloadPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                if (!Directory.Exists(extractPath))
                {
                    Directory.CreateDirectory(extractPath);
                }
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(url, downloadPath);

                    // Check if the file exists after downloading
                    if (File.Exists(downloadPath))
                    {
                        // File downloaded successfully
                        //MessageBox.Show("ZIP file downloaded successfully!");
                        await ExtractRarFileAndRunMsi(downloadPath, extractPath);
                        // Here you can perform actions like extracting the contents if needed
                        // Example: ExtractZipFile(downloadPath);
                    }
                    else
                    {
                        MessageBox.Show("Failed to download update file.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error downloading update file: {ex.Message}");
            }
        }
        private async Task ExtractRarFileAndRunMsi(string rarFilePath, string extractPath)
        {
            try
            {
                using (var archive = ArchiveFactory.Open(rarFilePath))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (!entry.IsDirectory)
                        {
                            entry.WriteToDirectory(extractPath, new ExtractionOptions()
                            {
                                ExtractFullPath = true,
                                Overwrite = true
                            });

                            // Check if the entry is an MSI file
                            if (Path.GetExtension(entry.Key).Equals(".msi", StringComparison.OrdinalIgnoreCase))
                            {
                                // Assuming there's only one MSI file in the archive
                                string msiFilePath = Path.Combine(extractPath, entry.Key);

                                // Start the extracted MSI file
                                await StartExtractedMsi(msiFilePath);
                                return;  // Stop processing after finding the first MSI file
                            }
                        }
                    }
                }

                //MessageBox.Show("RAR file extracted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error extracting update file: {ex.Message}");
            }
        }

        private async Task StartExtractedMsi(string msiFilePath)
        {
            try
            {
                // Start the extracted MSI file using Process.Start
                Process.Start(new ProcessStartInfo
                {
                    FileName = msiFilePath,
                    UseShellExecute = true
                });

                MessageBox.Show("Extracted update file started successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting extracted update file: {ex.Message}");
            }
        }

        private bool AuthenticateUser(string username, string password, out UserData userData)
        {
            try
            {
                string connectionString = "Server=202.131.4.20;Port=3306;Database=naadgaim_registration;User ID=naadgaim_naadgaim;Password=L0Lyumaa";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    // Check database connection
                    try
                    {
                        connection.Open();
                    }
                    catch (InvalidOperationException)
                    {
                        MessageBox.Show("Error connecting to MySQL database.");
                        isOffline = true;
                        userData = null;
                        return false;
                    }

                    string query = "SELECT * FROM UserData WHERE Username = @Username AND Password = @Password";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userData = new UserData
                                {
                                    Username = reader["Username"].ToString(),
                                    Id = reader["Id"].ToString(),
                                    lvl = Convert.ToInt32(reader["lvl"]),
                                    love = Convert.ToInt32(reader["love"]),
                                    premium = Convert.ToBoolean(reader["premium"]),
                                    premiumDate = reader["premiumDate"].ToString(),
                                    createdDate = reader["createdDate"].ToString()
                                };

                                // Assuming isOffline is a global variable
                                isOffline = false;

                                return true;
                            }
                        }
                    }
                }

                // Authentication failed
                userData = null;
                return false;
            }
            catch (MySqlException ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"MySQL Exception: {ex.Message}");

                if (ex.Number == 0)
                {
                    MessageBox.Show("MySQL server is down or unreachable.");
                }
                else
                {
                    MessageBox.Show($"Error connecting to MySQL server: {ex.Message}");
                }

                isOffline = true;
                userData = null;
                return false;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Exception: {ex.Message}");

                MessageBox.Show($"An error occurred: {ex.Message}");
                userData = null;
                isOffline = true;
                return false;
            }
        }






        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Check if stored credentials exist and populate the fields
            if (SecureStorage.TryGetStoredCredentials(out string storedUsername, out string storedPassword))
            {
                UsernameTextBox.Text = storedUsername;
                PasswordBox.Password = storedPassword;
                RememberMeCheckBox.IsChecked = true;
                AutoUpdater.Start("https://www.baaduu.me/version.xml");
                AutoUpdater.ShowSkipButton = false;
                AutoUpdater.ShowRemindLaterButton = false;
                AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
                AutoUpdater.InstalledVersion = new Version("0.0.7"); // Set your current application version
            }
        }


        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Open the default web browser with the specified URL
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "http://www.naadgai.mn",
                    UseShellExecute = true
                });
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                // Handle the exception, log it, or show an error message
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Close the window
        }
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginButton_Click(sender, e); // Call the login method
            }
        }
    }
  
}
public class ApiResponse
{
    public bool IsSuccess { get; set; }
    public UserData UserData { get; set; }
}