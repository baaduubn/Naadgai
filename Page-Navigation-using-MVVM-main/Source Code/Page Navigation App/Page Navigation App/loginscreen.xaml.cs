using System;
using System.Windows;

namespace Page_Navigation_App
{
    public partial class loginscreen : Window
    {
        public loginscreen()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the entered username and password
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            // Implement your authentication logic here
            bool isAuthenticated = AuthenticateUser(username, password);

            if (isAuthenticated)
            {
                // If authentication is successful, close the login window and open the main window
                MessageBox.Show("Амжилттай нэвтэрлээ!");

                // Close the login window

                // Open the MainWindow (replace with your actual main window name)
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Нэвтрэх нэр нууц үг буруу байна.");
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            // Replace this with your actual authentication logic (e.g., checking against a database)
            // For demonstration purposes, we assume a simple username and password here.
            // You should replace this with your own authentication mechanism.
            return username == "admin" && password == "admin";
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
