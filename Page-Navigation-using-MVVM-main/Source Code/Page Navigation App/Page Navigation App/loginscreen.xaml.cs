using System;
using System.Windows;
using System.Windows.Input;


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
                    MessageBox.Show("stored password.");
                }
            }
            else
            {
                MessageBox.Show("Нэвтрэх нэр нууц үг буруу байна.");
            }
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginButton_Click(sender, e); // Call the login method
            }
        }

        private bool AuthenticateUser(string username, string password ,out UserData userData)
        {
            // Replace this with your actual authentication logic (e.g., checking against a database)
            // For demonstration purposes, we assume a simple username and password here.
            // You should replace this with your own authentication mechanism.
            if (username == "admin" && password == "admin")
            {
                // Authentication successful, create a UserData object with user information
                userData = new UserData
                {
                    Username = "admin",
                    Id = "000000",
                    lvl = 99,
                    like = 999,
                    premium = true,
                    premiumDate ="2023-12-15",
                    createdDate ="2014-09-01"
                    // Add more user information as needed
                };
                return true;
            }
            if (username == "baaduu" && password == "baaduu")
            {
                // Authentication successful, create a UserData object with user information
                userData = new UserData
                {
                    Username = "baaduu",
                    Id = "000001",
                    lvl = 1,
                    like = 99,
                    premium = false,
                    premiumDate = "2021-02-06",
                    createdDate = "1997-05-31"
                    // Add more user information as needed
                };
                return true;
            }

            userData = null; // Authentication failed
            return false;
        }



        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            // Handle the "Forgot Password" link click event
            MessageBox.Show("Forgot Password clicked!");
            // Implement your "Forgot Password" functionality here
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Close the window
        }

    }
}
