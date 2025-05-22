using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using WinUI.Model;
using WinUI.Service;
using Microsoft.UI.Xaml;
using WinUI.Proxy;
using System.Linq;
using System.Collections.ObjectModel;
using ClassLibrary.Domain;

namespace WinUI.ViewModel
{
    public class DoctorViewModel : IDoctorViewModel, INotifyPropertyChanged
    {
        private const string DefaultDoctorName = "Loading...";
        private const string DefaultDepartmentName = "Loading department...";
        private const double DefaultRating = 0.0;
        private const string DefaultCareerInfo = "Loading career information...";
        private const string DefaultAvatarUrl = "https://picsum.photos/200";
        private const string DefaultPhoneNumber = "Loading phone...";
        private const string DefaultEmail = "Loading mail...";
        private const string DefaultDoctorAddress = "Loading address...";
        private const string NotSpecified = "Not specified";
        private const string NotAvailable = "N/A";
        private const string NotProvided = "Not provided";
        private const string DefaultNotFoundDoctorName = "Doctor profile not found";

        private const string DefaultErrorDoctorName = "Error loading profile";
        private const string DefaultErrorDepartmentName = "Error";
        private const string DefaultErrorCareerInfo = "Please try again later";

        private readonly IDoctorService _doctorService;

        public DoctorViewModel(IDoctorService doctorService, int userId)
        {
            _doctorService = doctorService;
            UserId = userId;

            DoctorName = DefaultDoctorName;
            DepartmentName = DefaultDepartmentName;
            Rating = DefaultRating;
            CareerInfo = DefaultCareerInfo;
            AvatarUrl = DefaultAvatarUrl;
            DoctorAddress = DefaultDoctorAddress;
            PhoneNumber = DefaultPhoneNumber;
            Mail = DefaultEmail;

            OriginalDoctor = DoctorModel.Default;
            IsLoading = false;
            LoadDepartments();
        }

        public DoctorViewModel(){}

        public event PropertyChangedEventHandler? PropertyChanged;

        public DoctorModel OriginalDoctor { get; set; }

        private int _userId;

        public int UserId
        {
            get => _userId;
            set
            {
                if (_userId != value)
                {
                    _userId = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _doctorName = string.Empty;

        public string DoctorName
        {
            get => _doctorName;
            set
            {
                if (_doctorName != value)
                {
                    _doctorName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _doctorAddress = string.Empty;
        public string DoctorAddress
        {
            get => _doctorAddress;
            set
            {
                if (_doctorAddress != value)
                {
                    _doctorAddress = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _departmentId;

        public int DepartmentId
        {
            get => _departmentId;
            set
            {
                if (_departmentId != value)
                {
                    _departmentId = value;
                    OnPropertyChanged();
                    DepartmentName = Departments.FirstOrDefault(d => d.departmentId == _departmentId)?.departmentName ?? DefaultDepartmentName;
                }
            }
        }

        private string _departmentName = string.Empty;

        public string DepartmentName
        {
            get => _departmentName;
            set
            {
                if (_departmentName != value)
                {
                    _departmentName = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _rating;

        public double Rating
        {
            get => _rating;
            set
            {
                if (_rating != value)
                {
                    _rating = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _careerInfo = string.Empty;

        public string CareerInfo
        {
            get => _careerInfo;
            set
            {
                if (_careerInfo != value)
                {
                    _careerInfo = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _avatarUrl = string.Empty;

        public string AvatarUrl
        {
            get => _avatarUrl;
            set
            {
                if (_avatarUrl != value)
                {
                    _avatarUrl = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _phoneNumber = string.Empty;

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _mail = string.Empty;

        public string Mail
        {
            get => _mail;
            set
            {
                if (_mail != value)
                {
                    _mail = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(MainContentVisibility));
                }
            }
        }

        public Visibility MainContentVisibility => IsLoading ? Visibility.Collapsed : Visibility.Visible;

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Department> Departments { get; set; } = new ObservableCollection<Department>();

        public async Task<bool> LoadDoctorInformationAsync(int userId)
        {
            System.Diagnostics.Debug.WriteLine("LoadDoctorInformationAsync called");
            try
            {
                IsLoading = true;
                System.Diagnostics.Debug.WriteLine("IsLoading set to true");

                // Use the service to load doctor info
                bool wasLoaded = await _doctorService.LoadDoctorInformationByUserId(userId);
                if (!wasLoaded)
                {
                    System.Diagnostics.Debug.WriteLine("Doctor info not loaded by service");
                    return false;
                }

                var info = _doctorService.DoctorInformation;
                DoctorName = info.DoctorName;
                DepartmentId = info.DepartmentId;
                DepartmentName = info.DepartmentName;
                Rating = info.Rating;
                PhoneNumber = info.PhoneNumber;
                Mail = info.Mail;

                OriginalDoctor.DoctorName = DoctorName;
                OriginalDoctor.DepartmentId = DepartmentId;
                OriginalDoctor.DepartmentName = DepartmentName;
                OriginalDoctor.Rating = Rating;
                OriginalDoctor.PhoneNumber = PhoneNumber;
                OriginalDoctor.Mail = Mail;

                return true;
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {exception.Message}");
                Debug.WriteLine($"Error in ViewModel: {exception.Message}");

                // Set error state
                DoctorName = DefaultErrorDoctorName;
                DepartmentName = DefaultErrorDepartmentName;
                CareerInfo = DefaultErrorCareerInfo;

                return false;
            }
            finally
            {
                IsLoading = false;
                System.Diagnostics.Debug.WriteLine("IsLoading set to false (finally)");
            }
        }


        public void RevertChanges()
        {
            DoctorName = OriginalDoctor.DoctorName;
            DepartmentName = OriginalDoctor.DepartmentName;
            DepartmentId = OriginalDoctor.DepartmentId;
            PhoneNumber = OriginalDoctor.PhoneNumber;
            Mail = OriginalDoctor.Mail;
        }

        

        public async Task<bool> UpdateDoctorFieldAsync(DoctorService.UpdateField field, string newValue, int? departmentId = null)
        {
            try
            {
                IsLoading = true;

                bool result;
                if (field == DoctorService.UpdateField.Department && departmentId.HasValue)
                {
                    result = await _doctorService.UpdateDoctorProfile(UserId, field, string.Empty, departmentId);
                }
                else
                {
                    result = await _doctorService.UpdateDoctorProfile(UserId, field, newValue);
                }

                if (result)
                {
                    switch (field)
                    {
                        case DoctorService.UpdateField.DoctorName:
                            DoctorName = newValue;
                            OriginalDoctor = OriginalDoctor.Clone(doctorName: newValue);
                            break;

                        case DoctorService.UpdateField.Department:
                            if (departmentId.HasValue)
                            {
                                DepartmentId = departmentId.Value;
                                OriginalDoctor = OriginalDoctor.Clone(departmentId: departmentId.Value);
                            }
                            break;

                        case DoctorService.UpdateField.CareerInfo:
                            CareerInfo = newValue;
                            OriginalDoctor = OriginalDoctor.Clone(careerInformation: newValue);
                            break;

                        case DoctorService.UpdateField.AvatarUrl:
                            AvatarUrl = newValue;
                            OriginalDoctor = OriginalDoctor.Clone(avatarUrl: newValue);
                            break;

                        case DoctorService.UpdateField.PhoneNumber:
                            PhoneNumber = newValue;
                            OriginalDoctor = OriginalDoctor.Clone(phoneNumber: newValue);
                            break;

                        case DoctorService.UpdateField.Mail:
                            Mail = newValue;
                            OriginalDoctor = OriginalDoctor.Clone(mail: newValue);
                            break;
                    }
                }

                return result;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }


        public async Task<(bool updateSuccessful, string? errorMessage)> TryUpdateDoctorProfileAsync()
        {
            try
            {
                if (OriginalDoctor == null)
                    return (false, "Original doctor data is not initialized.");

                bool changeMade = false;

                if (DoctorName != OriginalDoctor.DoctorName)
                    changeMade |= await UpdateDoctorFieldAsync(DoctorService.UpdateField.DoctorName, DoctorName);

                if (DepartmentId != OriginalDoctor.DepartmentId)
                    changeMade |= await UpdateDoctorFieldAsync(DoctorService.UpdateField.Department, string.Empty, DepartmentId);

                if (PhoneNumber != OriginalDoctor.PhoneNumber)
                    changeMade |= await UpdateDoctorFieldAsync(DoctorService.UpdateField.PhoneNumber, PhoneNumber);

                if (Mail != OriginalDoctor.Mail)
                    changeMade |= await UpdateDoctorFieldAsync(DoctorService.UpdateField.Mail, Mail);

                return (changeMade, null);
            }
            catch (Exception ex)
            {
                RevertChanges();
                return (false, ex.Message);
            }
        }
        private async void LoadDepartments()
        {
            try
            {
                var departments = await _doctorService.GetAllDepartments();
                Departments.Clear();
                foreach (var dept in departments)
                {
                    Departments.Add(dept);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading departments: {ex.Message}");
            }
        }
    }
}