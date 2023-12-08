using System;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Input;
using AutoUpdaterDotNET;

namespace Page_Navigation_App
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Set up AutoUpdater.NET
            AutoUpdater.Start("https://www.baaduu.me/version.xml");
            AutoUpdater.ShowSkipButton = false;
            AutoUpdater.ShowRemindLaterButton = false;
            // Hook up event handlers
            AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
            AutoUpdater.InstalledVersion = new Version("1.0.5.0"); // Set your current application version
        }
       
        private void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args.Error == null)
            {
                if (args.IsUpdateAvailable)
                {
                    MessageBoxResult dialogResult;
                    if (args.Mandatory.Value)
                    {
                        dialogResult = System.Windows.MessageBox.Show(
                            $@"Шинэ хувилбар  {args.CurrentVersion} байна. Та {args.InstalledVersion} хувилбарыг ашиглаж байна. Энэ бол шаардлагатай шинэчлэлт юм. Програмыг шинэчлэхийн тулд OK дарна уу.",
                            @"Шинэчлэх боломжтой", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        dialogResult = System.Windows.MessageBox.Show(
                            $@"Шинэ хувилбар байна {args.CurrentVersion} байна. Та {args.InstalledVersion} хувилбарыг ашиглаж байна.Та програмаа одоо шинэчлэхийг хүсч байна уу?",
                            @"Шинэчлэх боломжтой", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    }

                    // Uncomment the following line if you want to show the standard update dialog instead.
                     AutoUpdater.ShowUpdateForm(args);

                    if (dialogResult == MessageBoxResult.Yes || dialogResult == MessageBoxResult.OK)
                    {
                        try
                        {
                            if (AutoUpdater.DownloadUpdate(args))
                            {
                                
                                try
                                {
                                    MessageBox.Show("dsad");
                                    //string filePath = @"C:\Program Files\Supergem\view";
                                    //Process.Start(filePath);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("sda meen");
                                    //Console.WriteLine($"An error occurred: {ex.Message}");
                                }

                            }
                            else
                            {
                                MessageBox.Show("sda yah ged bgan");
                            }
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show("sda meen");
                            MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show(@"Шинэчлэлт байхгүй байна. Дараа дахин оролдож үзнэ үү.", @"Шинэчлэлт байхгүй байна",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                if (args.Error is WebException)
                {
                    System.Windows.MessageBox.Show(
                        @"Шинэчлэх серверт холбогдоход асуудал гарлаа. Интернэт холболтоо шалгаад дараа дахин оролдоно уу.",
                        @"Шинэчлэх шалгалт амжилтгүй боллоо", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(args.Error.Message, args.Error.GetType().ToString(), MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
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

        private void WindowDrag(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
