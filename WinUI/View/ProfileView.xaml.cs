using Microsoft.UI.Xaml.Controls;
using WinUI.ViewModel;

namespace WinUI.View
{
    public sealed partial class ProfileView : Page
    {
        public ProfileView(PatientViewModel patient_view_model)
        {
            this.InitializeComponent();

            this.DataContext = patient_view_model; // Replace with actual data model or ViewModel
        }
    }
}
