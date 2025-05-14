using System;
using Microsoft.UI.Xaml.Controls;
using WinUI.View;


/*
public static class NavigationService
{
    public static INavigationFrame sMainFrame { get; set; }

    public static void navigateToLogin()
    {
        sMainFrame?.Navigate(typeof(LogInView), null);
    }

    public static void navigate(Type page_type, object parameter = null)
    {
        sMainFrame?.Navigate(page_type, parameter);
    }
}
*/

namespace WinUI.Service
{
    public static class NavigationService
    {
        public static Frame sMainFrame { get; set; }

        public static void navigateToLogin()
        {
            if (sMainFrame != null)
            {
                sMainFrame.Navigate(typeof(LogInView), null);
            }
        }

        public static void navigate(Type page_type, object parameter = null)
        {
            if (sMainFrame != null)
            {
                sMainFrame.Navigate(page_type, parameter);
            }
        }
    }
}

