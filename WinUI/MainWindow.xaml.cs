using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.UI.Xaml.Navigation;
using ClassLibrary.Repository;
using WinUI.Proxy;
using WinUI.View;

namespace WinUI
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            root_frame.Navigate(typeof(LogInView), null);
        }

        void onNavigationFailed(object _sender, NavigationFailedEventArgs _nav_failed_event_args)
        {
            throw new Exception("Failed to load Page: " + _nav_failed_event_args.SourcePageType.FullName);
        }
    }
}
