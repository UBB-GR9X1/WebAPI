using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using ClassLibrary.Domain;
using ClassLibrary.Model;
using ClassLibrary.Repository;
using ClassLibrary.Service;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUI.Exceptions;
using WinUI.Model;
using WinUI.Proxy;
using WinUI.Service;
using WinUI.ViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateAccountView : Page
    {
        private AuthViewModel _view_model_create_account;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAccountView"/> class.
        /// </summary>
        public CreateAccountView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAccountView"/> class.
        /// </summary>
        /// <param name="auth_view_model">View Model for Creating an account.</param>
        public CreateAccountView(AuthViewModel auth_view_model)
        {
            this.InitializeComponent();
            this._view_model_create_account = auth_view_model;
        }

        /// <summary>
        /// Handle navigation parameters
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs navigate_event_args)
        {
            base.OnNavigatedTo(navigate_event_args);

            if (navigate_event_args.Parameter is AuthViewModel auth_view_model)
            {
                this._view_model_create_account = auth_view_model;
            }
        }

        private async void createAccountButtonClick(object sender, RoutedEventArgs routed_event_args)
        {
            string username = this.username_field.Text;
            string password = this.password_field.Password;
            string mail = this.email_text_box.Text;
            string name = this.name_text_box.Text;
            string emergency_contact = this.emergency_contact_text_box.Text;

            if (this.birth_date_calendar_picker.Date.HasValue)
            {
                DateOnly birth_date = DateOnly.FromDateTime(this.birth_date_calendar_picker.Date.Value.DateTime);
                this.birth_date_calendar_picker.Date = new DateTimeOffset(birth_date.ToDateTime(TimeOnly.MinValue));

                string cnp = this.cnp_textbox.Text;

                BloodType? selected_blood_type = null;
                string blood_type = "";
                if (this.blood_type_combo_box.SelectedItem is ComboBoxItem selected_item)
                {
                    string? selected_tag = selected_item.Tag.ToString();
                    if (selected_tag != null)
                    {
                        switch (selected_tag.Trim())
                        {
                            case "A_POSITIVE":
                                selected_blood_type = BloodType.A_POSITIVE;
                                blood_type = "A+";
                                break;
                            case "A_NEGATIVE":
                                selected_blood_type = BloodType.A_NEGATIVE;
                                blood_type = "A-";
                                break;
                            case "B_POSITIVE":
                                selected_blood_type = BloodType.B_POSITIVE;
                                blood_type = "B+";
                                break;
                            case "B_NEGATIVE":
                                selected_blood_type = BloodType.B_NEGATIVE;
                                blood_type = "B-";
                                break;
                            case "AB_POSITIVE":
                                selected_blood_type = BloodType.AB_POSITIVE;
                                blood_type = "AB+";
                                break;
                            case "AB_NEGATIVE":
                                selected_blood_type = BloodType.AB_NEGATIVE;
                                blood_type = "AB-";
                                break;
                            case "O_POSITIVE":
                                selected_blood_type = BloodType.O_POSITIVE;
                                blood_type = "O+";
                                break;
                            case "O_NEGATIVE":
                                selected_blood_type = BloodType.O_NEGATIVE;
                                blood_type = "O-";
                                break;
                        }
                    }
                }

                if (selected_blood_type == null)
                {
                    var validation_dialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = "Please select a blood type.",
                        CloseButtonText = "OK",
                    };

                    validation_dialog.XamlRoot = this.Content.XamlRoot;
                    await validation_dialog.ShowAsync();
                    return;
                }

                bool weight_valid = double.TryParse(this.weight_text_box.Text, out double weight);
                bool height_valid = int.TryParse(this.height_text_box.Text, out int height);

                if (!weight_valid || !height_valid || weight <= 0 || height <= 0)
                {
                    var validation_dialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = "Please enter valid Weight (kg) and Height (cm).",
                        CloseButtonText = "OK",
                    };

                    validation_dialog.XamlRoot = this.Content.XamlRoot;
                    await validation_dialog.ShowAsync();
                    return;
                }

                try
                {
                   int user_id = await this._view_model_create_account.createAccount(new UserCreateAccountModel(username, password, mail, name, birth_date, cnp, (BloodType)selected_blood_type, emergency_contact, weight, height));

                    ClassLibrary.Repository.IPatientRepository patientRepository = new WinUI.Proxy.PatientProxy(new HttpClient());
                    ILoggerService logger_service = new LoggerService(new LoggerProxy());
                    IPatientService patient_service = new PatientService(patientRepository, logger_service);
                    PatientViewModel patient_view_model = new PatientViewModel(patient_service, user_id);

                    string allergies = "";
                    await patient_view_model.CreatePatient(user_id, weight, height, emergency_contact, allergies, blood_type);

                    var validation_dialog = new ContentDialog
                    {
                        Title = "Success!",
                        Content = "Successfully Registered",
                        CloseButtonText = "OK",
                    };

                    validation_dialog.XamlRoot = this.Content.XamlRoot;
                    await validation_dialog.ShowAsync();
                    return;

                }
                catch (AuthenticationException error)
                {
                    var validation_dialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = $"{error.Message}",
                        CloseButtonText = "OK",
                        XamlRoot = this.Content.XamlRoot,
                    };
                    await validation_dialog.ShowAsync();
                }
            }
            else
            {
                var validation_dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Birth date is required.",
                    CloseButtonText = "OK",
                };

                validation_dialog.XamlRoot = this.Content.XamlRoot;
                await validation_dialog.ShowAsync();
            }
        }

        private void goBackButtonClick(object sender, RoutedEventArgs routed_event_args)
        {
            NavigationService.navigateToLogin();
        }
    }
}
