using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using CursorCloak.UI.Services;

namespace CursorCloak.UI
{
    public partial class App : System.Windows.Application
    {
        private static Mutex? _mutex = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            const string appName = "CursorCloak_SingleInstance_Mutex";
            bool createdNew;

            _mutex = new Mutex(true, appName, out createdNew);

            if (!createdNew)
            {
                // App is already running! Exiting the application
                System.Windows.MessageBox.Show("CursorCloak is already running.", "CursorCloak", MessageBoxButton.OK, MessageBoxImage.Information);
                Environment.Exit(0);
                return;
            }

            try
            {
                // Check if running as administrator
                bool isAdmin = false;
                try
                {
                    var identity = WindowsIdentity.GetCurrent();
                    var principal = new WindowsPrincipal(identity);
                    isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
                }
                catch
                {
                    // If we can't check, assume not admin
                    isAdmin = false;
                }

                if (!isAdmin)
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

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Dispatcher exception: {e.Exception}");
            e.Handled = true; // Prevent crash if possible
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Domain exception: {e.ExceptionObject}");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _mutex?.ReleaseMutex();
            _mutex?.Dispose();
            base.OnExit(e);
        }
    }
}
