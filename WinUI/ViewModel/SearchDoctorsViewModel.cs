using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WinUI.Model;
using WinUI.Service;

namespace WinUI.ViewModel
{
    public class SearchDoctorsViewModel : ISearchDoctorsViewModel, INotifyPropertyChanged
    {
        private readonly ISearchDoctorsService _searchDoctorsService;

        private string _departmentPartialName;
        private DoctorModel _selectedDoctor = DoctorModel.Default;
        private bool _isProfileOpen;
        private ObservableCollection<DoctorModel> _doctors;

        public ObservableCollection<DoctorModel> Doctors
        {
            get => _doctors;
            private set
            {
                _doctors = value;
                OnPropertyChanged();
            }
        }

        public string DepartmentPartialName
        {
            get => _departmentPartialName;
            set
            {
                _departmentPartialName = value;
                OnPropertyChanged();
            }
        }

        public DoctorModel SelectedDoctor
        {
            get => _selectedDoctor;
            set
            {
                _selectedDoctor = value;
                OnPropertyChanged();
            }
        }

        public bool IsProfileOpen
        {
            get => _isProfileOpen;
            set
            {
                _isProfileOpen = value;
                OnPropertyChanged();
            }
        }

        public SearchDoctorsViewModel(ISearchDoctorsService searchDoctorsService, string departmentPartialName)
        {
            _searchDoctorsService = searchDoctorsService;
            _departmentPartialName = departmentPartialName;
            _doctors = new ObservableCollection<DoctorModel>();
            _isProfileOpen = false;
        }

        public async Task LoadDoctors()
        {
            try
            {
                await _searchDoctorsService.LoadDoctors(_departmentPartialName);

                Doctors.Clear();
                foreach (var doctor in _searchDoctorsService.GetSearchedDoctors())
                {
                    Doctors.Add(doctor);
                }
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error loading doctors: {error.Message}");
            }
        }

        public void ShowDoctorProfile(DoctorModel doctor)
        {
            SelectedDoctor = doctor;
            IsProfileOpen = true;
        }

        public void CloseDoctorProfile()
        {
            IsProfileOpen = false;
            SelectedDoctor = DoctorModel.Default;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}