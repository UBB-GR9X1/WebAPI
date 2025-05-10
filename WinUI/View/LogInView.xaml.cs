using ClassLibrary.IRepository;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUI.Exceptions;
using WinUI.Proxy;
using WinUI.Repository;
using WinUI.Service;
using WinUI.ViewModel;
using IPatientRepository = ClassLibrary.IRepository.IPatientRepository;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI.View
{
    /// <summary>
    /// The loggin window for the hospital application:
    /// Asks for a username and password
    /// If the user does not have an account there is a button for creating one.
    /// </summary>
    public sealed partial class LogInView : Page
    {
        private readonly IAuthViewModel _login_page_view_model;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginWindow"/> class.
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public LogInView()
        {
            this.InitializeComponent();

            ILogInRepository _log_in_service = new LogInProxy(new System.Net.Http.HttpClient());
            IAuthService _service = new AuthService(_log_in_service);
            this._login_page_view_model = new AuthViewModel(_service);

            NavigationService.sMainFrame = this.LoginFrame;

            this.LoginPanel.Visibility = Visibility.Visible;
            // Create login form page and navigate to it
            // this.mainFrame.navigate(typeof(LoginPage), this.loginPageViewModel);
        }

        /// <summary>
        /// Called when navigated to this page.
        /// </summary>
        /// <param name="_navigation_evnt_args">Navigation event arguments</param>
        protected override void OnNavigatedTo(NavigationEventArgs _navigation_evnt_args)
        {
            base.OnNavigatedTo(_navigation_evnt_args);

            // Reset UI state
            this.UsernameTextField.Text = string.Empty;
            this.PasswordTextField.Password = string.Empty;

            // Show login panel
            this.LoginPanel.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// It gets the text inside the username Text Block and the password Text Block,
        /// and if the user is not existent it shows an error, otherwise:
        /// Depending on the user, if it is patient or not, it sends the user to the Patient Window
        /// or to the Doctor Window.
        /// </summary>
        /// <param name="sender">.</param>
        /// <param name="e">..</param>
        private async void loginButtonClick(object sender, RoutedEventArgs e)
        {
            string _username = this.UsernameTextField.Text;
            string _password = this.PasswordTextField.Password;

            try
            {
                await this._login_page_view_model.login(_username, _password);

                this.LoginPanel.Visibility = Visibility.Collapsed;

                var _debug_dialog = new ContentDialog
                {
                    Title = "GOOD",
                    Content = $"LOGGED IN!",
                    CloseButtonText = "OK",
                };

                _debug_dialog.XamlRoot = this.Content.XamlRoot;
                await _debug_dialog.ShowAsync();

                // TODO: UPDATE
                if (this._login_page_view_model.getUserRole() == "Patient")
                {
                    WinUI.Repository.IPatientRepository patientRepository = new WinUI.Proxy.PatientProxy(new HttpClient());
                    ILoggerService loggerService = new LoggerService(new LoggerProxy());
                    IPatientService patientService = new PatientService(patientRepository, loggerService);
                    PatientViewModel patientViewModel = new PatientViewModel(patientService, this._login_page_view_model.authService.allUserInformation.userId);

                    var parameters = new Tuple<IPatientViewModel, IAuthViewModel>(patientViewModel, this._login_page_view_model);
                    NavigationService.navigate(typeof(PatientDashboardView), parameters);
                    return;
                }
                //else if (this.loginPageViewModel.GetUserRole() == "Doctor")
                //{
                //    IDoctorRepository doctorRepository = new DoctorRepository();
                //    IDoctorService doctorService = new DoctorService(doctorRepository);
                //    IDoctorViewModel doctorViewModel = new DoctorViewModel(doctorService, this.loginPageViewModel.AuthService.allUserInformation.user_id);

                //    var parameters = new Tuple<IDoctorViewModel, AuthViewModel>(doctorViewModel, this.loginPageViewModel);
                //    this.mainFrame.navigate(typeof(DoctorDashboardPage), parameters);
                //    return;
                //}
                
                //  Admin Dashboard is done
                else if (this._login_page_view_model.getUserRole() == "Admin")
                {
                       ILogRepository _logger_repository = new LoggerProxy();
                       Tuple<IAuthViewModel, ILogRepository> _parameters = new Tuple<IAuthViewModel, ILogRepository>(this._login_page_view_model, _logger_repository);
                       NavigationService.navigate(typeof(AdminDashboardPage), _parameters);
                       return;
                }
            }
            catch (AuthenticationException _new_authentication_exception)
            {
                var _validation_dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"{_new_authentication_exception.Message}",
                    CloseButtonText = "OK",
                };

                _validation_dialog.XamlRoot = this.Content.XamlRoot;
                await _validation_dialog.ShowAsync();
            }
        }

        private void createAccountButtonClick(object _sender, RoutedEventArgs _route_event_args)
        {
            this.LoginPanel.Visibility = Visibility.Collapsed;
            NavigationService.navigate(typeof(CreateAccountView), this._login_page_view_model);
        }
    }
}
