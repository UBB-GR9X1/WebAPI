using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using ClassLibrary.Domain;
using WinUI.Repository;
using ClassLibrary.Repository;
using WinUI.Proxy;
using Microsoft.UI.Xaml.Navigation;
using ClassLibrary.Repository;
using WinUI.Proxy;
using WinUI.View;

namespace WinUI
{
    public sealed partial class MainWindow : Window
    {
        // HttpClient instance to make HTTP requests
        private static readonly HttpClient client = new HttpClient();

        
        public MainWindow()
        {
            this.InitializeComponent();

            root_frame.Navigate(typeof(LogInView), null);
        }

        void onNavigationFailed(object _sender, NavigationFailedEventArgs _nav_failed_event_args)
        {
            try
            {
                INotificationRepository NotificationProxy = new NotificationProxy(new HttpClient());
                // Make the GET request to your WebAPI
                var response = NotificationProxy.getAllNotificationsAsync();


                // Update the Button content with the response
                //myButton.Content = response[0]; // should print Domain.Notification since we dont have a ToString() method;
            }
            catch (Exception ex)
            {
                // Handle error (e.g., if the server is not running or there's no internet connection)
                //myButton.Content = "Failed to load data";
                //responseText.Text = $"Error: {ex.Message}";
            }
            throw new Exception("Failed to load Page: " + _nav_failed_event_args.SourcePageType.FullName);
        }
    }
}
