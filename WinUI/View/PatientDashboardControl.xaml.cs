using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUI.Model;
using WinUI.View;
using WinUI.ViewModel;
using WinUI.Proxy;
using WinUI.Service;

namespace WinUI.View
{
    public sealed partial class PatientDashboardControl : UserControl
    {
        private IPatientViewModel? _patient_view_model;
        public event Action? LogoutButtonClicked;

        public PatientDashboardControl()
        {
            this.InitializeComponent();
        }

        public PatientDashboardControl(IPatientViewModel _patient_view_model)
        {
            InitializeComponent();
            this._patient_view_model = _patient_view_model;
            DataContext = this._patient_view_model;

            // Add logging to confirm if the DataContext is set correctly
            Debug.WriteLine($"DataContext set to: {_patient_view_model?.GetType().Name}");
        }
        public void NotificationsButton_Click(object sender, RoutedEventArgs e)
        {
            var notificationWindow = new NotificationWindow(new NotificationViewModel(new NotificationService(new NotificationProxy(new System.Net.Http.HttpClient())), this._patient_view_model.userId));

            // Show the window
            notificationWindow.Activate();  // Activate the window to display it
        }

        private async void OnUpdateButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this._patient_view_model == null)
                    throw new Exception("Patient is not initialized");

                bool has_changes = false;
                bool password_changed = false;

                // Check if data is different before updating
                if (this._patient_view_model.emergency_contact != this._patient_view_model.original_patient.emergencyContact)
                {
                    bool emergencyUpdated = await this._patient_view_model.updateEmergencyContact(this._patient_view_model.emergency_contact);
                    has_changes |= emergencyUpdated;
                }

                if (this._patient_view_model.weight != this._patient_view_model.original_patient.weight)
                {
                    bool weight_updated = await this._patient_view_model.updateWeight(this._patient_view_model.weight);
                    has_changes |= weight_updated;
                }

                if (this._patient_view_model.height != this._patient_view_model.original_patient.height)
                {
                    bool height_updated = await this._patient_view_model.updateHeight(this._patient_view_model.height);
                    has_changes |= height_updated;
                }

                if (this._patient_view_model.password != this._patient_view_model.original_patient.password)
                {
                    bool password_updated = await this._patient_view_model.updatePassword(this._patient_view_model.password);
                    has_changes |= password_updated;
                    password_changed = password_updated;
                }

                if (this._patient_view_model.name != this._patient_view_model.original_patient.patientName)
                {
                    bool name_updated = await this._patient_view_model.updateName(this._patient_view_model.name);
                    has_changes |= name_updated;
                }

                if (this._patient_view_model.address != this._patient_view_model.original_patient.address)
                {
                    bool address_updated = await this._patient_view_model.updateAddress(this._patient_view_model.address);
                    has_changes |= address_updated;
                }

                if (this._patient_view_model.phone_number != this._patient_view_model.original_patient.phoneNumber)
                {
                    bool phone_number_updated = await this._patient_view_model.updatePhoneNumber(this._patient_view_model.phone_number);
                    has_changes |= phone_number_updated;
                }

                if (this._patient_view_model.allergies != this._patient_view_model.original_patient.allergies)
                {
                    bool allergies_updated = await this._patient_view_model.updateAllergies(this._patient_view_model.allergies);
                    has_changes |= allergies_updated;
                }

                if (this._patient_view_model.blood_type != this._patient_view_model.original_patient.bloodType)
                {
                    bool blood_type_updated = await this._patient_view_model.updateBloodType(this._patient_view_model.blood_type);
                    has_changes |= blood_type_updated;
                }

                if (has_changes)
                {
                    await this._patient_view_model.logUpdate(this._patient_view_model.userId, ActionType.UPDATE_PROFILE);
                    if (password_changed) await this._patient_view_model.logUpdate(this._patient_view_model.userId, ActionType.CHANGE_PASSWORD);
                    await ShowDialogAsync("Success", "Changes applied successfully.");
                }
                else
                {
                    await ShowDialogAsync("No Changes", "Please modify the fields you want to update.");
                }
            }
            catch (Exception exception)
            {
                if (this._patient_view_model != null)
                {
                    restoreOriginalPatientData();
                    await ShowDialogAsync("Error", exception.Message);

                    // Log the error to debug
                    Debug.WriteLine($"Error: {exception.Message}");

                    // Re-load patient data in case of failure
                    await this._patient_view_model.loadPatientInfoByUserIdAsync(this._patient_view_model.userId);
                }
            }
        }

        private void restoreOriginalPatientData()
        {
            // Ensure the restored data comes from the original patient data
            PatientJointModel? original = this._patient_view_model!.original_patient;
            this._patient_view_model!.emergency_contact = original.emergencyContact;
            this._patient_view_model.weight = original.weight;
            this._patient_view_model.height = original.height;
            this._patient_view_model.blood_type = original.bloodType;
            this._patient_view_model.allergies = original.allergies;
            this._patient_view_model.name = original.patientName;
            this._patient_view_model.address = original.address;
            this._patient_view_model.phone_number = original.phoneNumber;

            // Log restoration to verify
            Debug.WriteLine("Restored original patient data.");
        }

        private async Task ShowDialogAsync(string _title, string _message)
        {
            var dialog = new ContentDialog
            {
                Title = _title,
                Content = _message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            await dialog.ShowAsync();
        }

        private void OnLogoutButtonClick(object sender, RoutedEventArgs e)
        {
            LogoutButtonClicked?.Invoke();
        }
    }
}
