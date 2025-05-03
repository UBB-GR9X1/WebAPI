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
using System.Security.Authentication;
using System.Threading.Tasks;

namespace WinUI.View
{
    public sealed partial class PatientDashboardView : Page
    {
        // TODO after having the authentication view model

        // private IAuthViewModel _authenticationViewModel;

        public PatientDashboardView()
        {
            this.InitializeComponent();
        }

        //public PatientDashboardView(IPatientViewModel patientViewModel, IAuthViewModel authenticationViewModel)
        //{
        //    InitializeComponent();
        //    _authenticationViewModel = authenticationViewModel;

        //    var patientDashboardControl = new PatientDashboardControl(patientViewModel);
        //    patientDashboardControl.LogoutButtonClicked += HandleLogoutRequested;

        //    this.Content = patientDashboardControl;
        //}

        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    base.OnNavigatedTo(e);

        //    if (e.Parameter is Tuple<IPatientViewModel, IAuthViewModel> parameters)
        //    {
        //        var patientViewModel = parameters.Item1;
        //        _authenticationViewModel = parameters.Item2;

        //        var patientDashboardControl = new PatientDashboardControl(patientViewModel);
        //        patientDashboardControl.LogoutButtonClicked += HandleLogoutRequested;

        //        this.Content = patientDashboardControl;
        //    }
        //}

        private async void HandleLogoutRequested()
        {
            try
            {
                //await _authenticationViewModel.Logout();

                //if (App.MainWindow is LoginWindow loginWindow)
                //{
                //    loginWindow.ReturnToLogin();
                //}
            }
            catch (AuthenticationException authenticationException)
            {
                await ShowErrorDialog("Authentication Error", authenticationException.Message);
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
