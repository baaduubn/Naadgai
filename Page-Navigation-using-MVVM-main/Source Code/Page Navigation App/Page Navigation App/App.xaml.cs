using System;
using System.Threading;
using System.Windows;

namespace Page_Navigation_App
{
    public partial class App : Application
    {
        private static Mutex mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            const string mutexName = "YourApplicationMutexName";
            bool createdNew;

            // Attempt to create a mutex with a unique name
            mutex = new Mutex(true, mutexName, out createdNew);

            if (!createdNew)
            {
                // Another instance is already running
                MessageBox.Show("Another instance of the application is already running.", "Application Already Running", MessageBoxButton.OK, MessageBoxImage.Information);
                Current.Shutdown();
            }

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Release the mutex on application exit
            mutex.ReleaseMutex();
            base.OnExit(e);
        }
    }
}
