using System;
using System.Configuration;
using System.Data;
using System.Security.Principal;
using System.Windows;
using System.Windows.Threading;
using CursorCloak.UI;

namespace CursorCloak.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
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

    protected override void OnExit(ExitEventArgs e)
    {
        GlobalMouseHook.Uninstall();
        base.OnExit(e);
    }

    private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        try
        {
            System.Windows.MessageBox.Show($"An unexpected error occurred: {e.Exception.Message}\n\nThe application will continue to run.", 
                          "CursorCloak Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            e.Handled = true;
        }
        catch
        {
            // If we can't even show a message box, just log and exit gracefully
            Environment.Exit(1);
        }
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        try
        {
            var exception = e.ExceptionObject as Exception;
            System.Windows.MessageBox.Show($"A fatal error occurred: {exception?.Message ?? "Unknown error"}\n\nThe application will now close.", 
                          "CursorCloak Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch
        {
            // Silent exit if we can't show message
        }
        finally
        {
            Environment.Exit(1);
        }
    }
}

