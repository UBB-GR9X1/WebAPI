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
using System.Threading.Tasks;
using WinUI.Model;
using WinUI.ViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchDoctorsDashboard : Page
    {
        public SearchDoctorsViewModel ViewModel { get; }

        public SearchDoctorsDashboard(SearchDoctorsViewModel viewModel)
        {
            this.InitializeComponent();
            ViewModel = viewModel;

            // Load doctors when page initializes
            this.Loaded += SearchDoctorsDashboard_Loaded;
        }

        private async void SearchDoctorsDashboard_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadDoctors();
        }

        private async Task LoadDoctors()
        {
            try
            {
                await ViewModel.LoadDoctors();
            }
            catch (Exception ex)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Error",
                    Content = $"Failed to load doctors: {ex.Message}",
                    PrimaryButtonText = "OK",
                    DefaultButton = ContentDialogButton.Primary
                };

                await errorDialog.ShowAsync();
            }
        }

        private async void OnSearch_Click(object sender, RoutedEventArgs e)
        {
            await LoadDoctors();
        }

        private void OnViewProfile_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is DoctorModel doctor)
            {
                ViewModel.ShowDoctorProfile(doctor);
            }
        }

        private void OnCloseProfile_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CloseDoctorProfile();
        }
    }
}
