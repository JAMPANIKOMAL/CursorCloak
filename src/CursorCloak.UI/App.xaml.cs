using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using CursorCloak.UI.Services;

namespace CursorCloak.UI
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// Handles application startup, single-instance enforcement, and global exception handling.
    /// </summary>
    public partial class App : System.Windows.Application
    {
        /// <summary>
        /// Mutex used to enforce single instance of the application.
        /// </summary>
        private static Mutex? _mutex = null;

        /// <summary>
        /// Unique name for the application mutex.
        /// </summary>
        private const string AppMutexName = "CursorCloak_SingleInstance_Mutex";

        /// <summary>
        /// Handles the startup event of the application.
        /// Checks for existing instances and administrator privileges.
        /// </summary>
        /// <param name="e">Startup event arguments.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            bool createdNew;
            _mutex = new Mutex(true, AppMutexName, out createdNew);

            if (!createdNew)
            {
                // App is already running! Exiting the application
                System.Windows.MessageBox.Show("CursorCloak is already running.", "CursorCloak", MessageBoxButton.OK, MessageBoxImage.Information);
                Environment.Exit(0);
                return;
            }

            try
            {
                if (!IsRunningAsAdministrator())
                {
                    System.Windows.MessageBox.Show("CursorCloak requires administrator privileges to function properly.\n\n" +
                                       "Please run the application as administrator.",
                                       "Administrator Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Environment.Exit(1);
                    return;
                }

                // Set up global exception handlers
                this.DispatcherUnhandledException += App_DispatcherUnhandledException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                
                // Add diagnostic logging
                System.Diagnostics.Debug.WriteLine("CursorCloak.UI starting...");

                // Install the global mouse hook ONCE for the whole app lifetime
                GlobalMouseHook.Install(() =>
                {
                    if (CursorCloak.UI.MainWindow.CurrentInstance != null)
                    {
                        CursorCloak.UI.MainWindow.CurrentInstance.OnGlobalMouseMove();
                    }
                });

                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                // Log startup failure
                System.Diagnostics.Debug.WriteLine($"Startup failed: {ex}");
                System.Windows.MessageBox.Show($"Failed to start application: {ex.Message}", "Startup Error", 
                              MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Checks if the current process is running with administrator privileges.
        /// </summary>
        /// <returns>True if running as administrator; otherwise, false.</returns>
        private bool IsRunningAsAdministrator()
        {
            try
            {
                var identity = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Handles unhandled exceptions on the UI dispatcher thread.
        /// </summary>
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Dispatcher exception: {e.Exception}");
            e.Handled = true; // Prevent crash if possible
        }

        /// <summary>
        /// Handles unhandled exceptions in the current application domain.
        /// </summary>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Domain exception: {e.ExceptionObject}");
        }

        /// <summary>
        /// Handles the exit event of the application.
        /// Releases resources and the single-instance mutex.
        /// </summary>
        /// <param name="e">Exit event arguments.</param>
        protected override void OnExit(ExitEventArgs e)
        {
            CursorEngine.Cleanup();
            _mutex?.ReleaseMutex();
            _mutex?.Dispose();
            base.OnExit(e);
        }
    }
}
