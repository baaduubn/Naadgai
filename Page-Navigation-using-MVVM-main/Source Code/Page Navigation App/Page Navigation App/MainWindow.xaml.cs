using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Xml;
using AutoUpdaterDotNET;
using SharpCompress.Archives;
using SharpCompress.Common;

namespace Page_Navigation_App
{
    public partial class MainWindow : Window
    {
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        public MainWindow()
        {
            InitializeComponent();
            AutoUpdater.Start("https://www.baaduu.me/version.xml");
            AutoUpdater.ShowSkipButton = false;
            AutoUpdater.ShowRemindLaterButton = false;
            AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
            AutoUpdater.InstalledVersion = new Version("0.0.5.1"); // Set your current application version
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

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinimizeApp_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ReleaseCapture();
                SendMessage(new WindowInteropHelper(this).Handle, WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero);
            }
        }

        private void Btn_Checked(object sender, RoutedEventArgs e)
        {

        }

        // ...
    }
}
