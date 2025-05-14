using System;
using Microsoft.UI.Xaml.Controls;
using WinUI.View;

namespace WinUI.Service
{
    public static class NavigationService
    {
        public static Frame s_main_frame { get; set; }

        public static void navigateToLogin()
        {
            if (s_main_frame != null)
            {
                s_main_frame.Navigate(typeof(LogInView), null);
            }
        }

        public static void navigate(Type page_type, object parameter = null)
        {
            if (s_main_frame != null)
            {
                s_main_frame.Navigate(page_type, parameter);
            }
        }
    }
}

