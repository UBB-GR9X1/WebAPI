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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    ///
    public sealed partial class DoctorDashboard : Page
    {
        public IDoctorViewModel ViewModel { get; set; }

        public DoctorDashboard()
        {
            this.InitializeComponent();
            ViewModel = new DoctorViewModel();
        }

        protected override async void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Tuple<IDoctorViewModel, IAuthViewModel> parameters)
            {
                ViewModel = parameters.Item1;
                this.DataContext = this;
                await ViewModel.LoadDoctorInformationAsync(ViewModel.UserId);
            }
            else if (e.Parameter is IDoctorViewModel vm)
            {
                ViewModel = vm;
                this.DataContext = this;
                await ViewModel.LoadDoctorInformationAsync(ViewModel.UserId);
            }
            System.Diagnostics.Debug.WriteLine("OnNavigatedTo called in DoctorDashboard");
            System.Diagnostics.Debug.WriteLine($"ViewModel is null: {ViewModel == null}");
            System.Diagnostics.Debug.WriteLine($"MainContentVisibility: {ViewModel?.MainContentVisibility}");
            // Optionally handle error if parameter is not as expected
        }

        private void OnRevertChanges(object sender, RoutedEventArgs e)
        {
            ViewModel.RevertChanges();
        }

        private async void OnSaveProfile(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Title = "Saving Profile",
                Content = "Saving your profile changes...",
                PrimaryButtonText = "OK",
                DefaultButton = ContentDialogButton.Primary
            };

            try
            {
                var (updateSuccessful, errorMessage) = await ViewModel.TryUpdateDoctorProfileAsync();

                if (updateSuccessful)
                {
                    dialog.Title = "Success";
                    dialog.Content = "Your profile has been updated successfully.";
                }
                else
                {
                    dialog.Title = "Error";
                    dialog.Content = $"Failed to update profile: {errorMessage ?? "Unknown error"}";
                }
            }
            catch (Exception ex)
            {
                dialog.Title = "Error";
                dialog.Content = $"An error occurred: {ex.Message}";
            }

            await dialog.ShowAsync();
        }
    }
}
