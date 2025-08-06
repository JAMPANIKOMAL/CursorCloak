using System;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;

namespace CursorCloak.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // Set up global exception handlers
        this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        
        base.OnStartup(e);
    }

    private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        try
        {
            MessageBox.Show($"An unexpected error occurred: {e.Exception.Message}\n\nThe application will continue to run.", 
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
            MessageBox.Show($"A fatal error occurred: {exception?.Message ?? "Unknown error"}\n\nThe application will now close.", 
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

