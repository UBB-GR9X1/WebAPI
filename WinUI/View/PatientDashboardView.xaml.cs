using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using WinUI.ViewModel;
using WinUI.View;
using System.Security.Authentication;
using System.Threading.Tasks;
using WinUI.Service;

namespace WinUI.View
{
    public sealed partial class PatientDashboardView : Page
    {
        private AuthViewModel _authentication_view_model;

        public PatientDashboardView()
        {
            this.InitializeComponent();
        }

        public PatientDashboardView(PatientViewModel _patient_view_model, AuthViewModel _authentication_view_model)
        {
            InitializeComponent();
            this._authentication_view_model = _authentication_view_model;

            var patient_dashboard_control = new PatientDashboardControl(_patient_view_model);
            patient_dashboard_control.logout_button_clicked += handleLogoutRequested;

            this.Content = patient_dashboard_control;
        }

        protected override void OnNavigatedTo(NavigationEventArgs navigation_event)
        {
            base.OnNavigatedTo(navigation_event);

            if (navigation_event.Parameter is Tuple<PatientViewModel, AuthViewModel> parameters)
            {
                PatientViewModel patient_view_model = parameters.Item1;
                this._authentication_view_model = parameters.Item2;

                var patient_dashboard_control = new PatientDashboardControl(patient_view_model);
                patient_dashboard_control.logout_button_clicked += handleLogoutRequested;

                this.Content = patient_dashboard_control;
            }
        }

        private async void handleLogoutRequested()
        {
            try
            {
                await this._authentication_view_model.logout();
                NavigationService.navigateToLogin();
            }
            catch (AuthenticationException authentication_exception)
            {
                await ShowErrorDialog("Authentication Error", authentication_exception.Message);
            }
            catch (Exception exception)
            {
                await ShowErrorDialog("Error", exception.Message);
            }
        }

        private async Task ShowErrorDialog(string title, string message)
        {
            var errorDialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            await errorDialog.ShowAsync();
        }
    }
}
