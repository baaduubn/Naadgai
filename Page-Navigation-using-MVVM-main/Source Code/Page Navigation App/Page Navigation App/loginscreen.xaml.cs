using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using MySql.Data.MySqlClient;

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
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            bool rememberMe = RememberMeCheckBox.IsChecked ?? false;
            if (AuthenticateUser(username, password, out UserData userData))
            {
                AppData.CurrentUser = userData; // Save the user data globally

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
                    MessageBox.Show($"Сервертэй холбогдоход алдаа гарлаа");
                }
                else
                {
                    MessageBox.Show("Нэвтрэх нэр нууц үг буруу байна.");
                }

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
    //private bool AuthenticateUser(string username, string password ,out UserData userData)
    //{
    //    // Replace this with your actual authentication logic (e.g., checking against a database)
    //    // For demonstration purposes, we assume a simple username and password here.
    //    // You should replace this with your own authentication mechanism.
    //    if (username == "admin" && password == "admin")
    //    {
    //        // Authentication successful, create a UserData object with user information
    //        userData = new UserData
    //        {
    //            Username = "admin",
    //            Id = "000000",
    //            lvl = 99,
    //            like = 999,
    //            premium = true,
    //            premiumDate ="2023-12-15",
    //            createdDate ="2014-09-01"
    //            // Add more user information as needed
    //        };
    //        return true;
    //    }
    //    if (username == "baaduu" && password == "baaduu")
    //    {
    //        // Authentication successful, create a UserData object with user information
    //        userData = new UserData
    //        {
    //            Username = "baaduu",
    //            Id = "000001",
    //            lvl = 1,
    //            like = 99,
    //            premium = false,
    //            premiumDate = "2021-02-06",
    //            createdDate = "1997-05-31"
    //            // Add more user information as needed
    //        };
    //        return true;
    //    }

    //    userData = null; // Authentication failed
    //    return false;
    //}
}
