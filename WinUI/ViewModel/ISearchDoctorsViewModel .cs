using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WinUI.Model;

public interface ISearchDoctorsViewModel
{
    ObservableCollection<DoctorModel> Doctors { get; }
    string DepartmentPartialName { get; set; }
    DoctorModel SelectedDoctor { get; set; }
    bool IsProfileOpen { get; set; }

    Task LoadDoctors();
    void ShowDoctorProfile(DoctorModel doctor);
    void CloseDoctorProfile();
}