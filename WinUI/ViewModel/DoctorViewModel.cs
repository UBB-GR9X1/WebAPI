using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using WinUI.Model;
using WinUI.Service;

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
            PhoneNumber = DefaultPhoneNumber;
            Mail = DefaultEmail;

            OriginalDoctor = DoctorModel.Default;
            _ = LoadDoctorInformationAsync(userId);
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
                }
            }
        }

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<bool> LoadDoctorInformationAsync(int userId)
        {
            try
            {
                IsLoading = true;

                bool wasLoaded = await _doctorService.LoadDoctorInformationByUserId(userId);

                if (wasLoaded && _doctorService.DoctorInformation != DoctorModel.Default)
                {
                    var doctor = _doctorService.DoctorInformation;
                    DoctorName = doctor.DoctorName ?? NotSpecified;
                    DepartmentId = doctor.DepartmentId;
                    DepartmentName = doctor.DepartmentName ?? NotAvailable;
                    Rating = doctor.Rating > 0 ? doctor.Rating : DefaultRating;
                    CareerInfo = doctor.CareerInfo ?? NotAvailable;
                    AvatarUrl = doctor.AvatarUrl ?? DefaultAvatarUrl;
                    PhoneNumber = doctor.PhoneNumber ?? NotProvided;
                    Mail = doctor.Mail ?? NotProvided;

                    OriginalDoctor = new DoctorModel(-1, DoctorName, DepartmentId, DepartmentName, Rating, CareerInfo, AvatarUrl,
                        PhoneNumber, Mail);

                    return true;
                }

                // Set not found state
                DoctorName = DefaultNotFoundDoctorName;
                DepartmentName = NotAvailable;
                return false;
            }
            catch (Exception exception)
            {
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
            }
        }

        public async Task<bool> UpdateDoctorNameAsync(string newName)
        {
            return await UpdateFieldAsync(() => _doctorService.UpdateDoctorName(UserId, newName), 
                () => DoctorName = newName,
                () => OriginalDoctor.Clone(doctorName: newName)
            );
        }

        public async Task<bool> UpdateDepartmentAsync(int newDepartmentId)
        {
            return await UpdateFieldAsync(() => _doctorService.UpdateDepartment(UserId, newDepartmentId), 
                () => DepartmentId = newDepartmentId, 
                () => OriginalDoctor.Clone(departmentId: newDepartmentId)
            );
        }

        public async Task<bool> UpdateCareerInfoAsync(string newCareerInformation)
        {
            return await UpdateFieldAsync(() => _doctorService.UpdateCareerInfo(UserId, newCareerInformation), 
                () => CareerInfo = newCareerInformation, 
                () => OriginalDoctor.Clone(careerInformation: newCareerInformation) 
            );
        }

        public async Task<bool> UpdateAvatarUrlAsync(string newAvatarUrl)
        {
            return await UpdateFieldAsync(() => _doctorService.UpdateAvatarUrl(UserId, newAvatarUrl), 
                () => AvatarUrl = newAvatarUrl, 
                () => OriginalDoctor.Clone(avatarUrl: newAvatarUrl)
            );
        }

        public async Task<bool> UpdatePhoneNumberAsync(string newPhoneNumber)
        {
            return await UpdateFieldAsync(() => _doctorService.UpdatePhoneNumber(UserId, newPhoneNumber),
                () => PhoneNumber = newPhoneNumber, 
                () => OriginalDoctor.Clone(phoneNumber: newPhoneNumber) 
            );
        }


        public async Task<bool> UpdateMailAsync(string newEmail)
        {
            return await UpdateFieldAsync(
                () => _doctorService.UpdateEmail(UserId, newEmail), 
                () => Mail = newEmail, 
                () => OriginalDoctor.Clone(email: newEmail) 
            );
        }


        public void RevertChanges()
        {
            DoctorName = OriginalDoctor.DoctorName;
            DepartmentName = OriginalDoctor.DepartmentName;
            CareerInfo = OriginalDoctor.CareerInfo;
            AvatarUrl = OriginalDoctor.AvatarUrl;
            PhoneNumber = OriginalDoctor.PhoneNumber;
            Mail = OriginalDoctor.Mail;
        }

        public async Task<(bool updateSuccessful, string? errorMessage)> TryUpdateDoctorProfileAsync()
        {
            try
            {
                if (OriginalDoctor == null)
                    return (false, "Original doctor data is not initialized.");

                bool changeMade = false;

                if (DoctorName != OriginalDoctor.DoctorName)
                    changeMade |= await UpdateDoctorNameAsync(DoctorName);

                if (DepartmentName != OriginalDoctor.DepartmentName)
                    changeMade |= await UpdateDepartmentAsync(DepartmentId);

                if (CareerInfo != OriginalDoctor.CareerInfo)
                    changeMade |= await UpdateCareerInfoAsync(CareerInfo);

                if (AvatarUrl != OriginalDoctor.AvatarUrl)
                    changeMade |= await UpdateAvatarUrlAsync(AvatarUrl);

                if (PhoneNumber != OriginalDoctor.PhoneNumber)
                    changeMade |= await UpdatePhoneNumberAsync(PhoneNumber);

                if (Mail != OriginalDoctor.Mail)
                    changeMade |= await UpdateMailAsync(Mail);


                return (changeMade, null);
            }
            catch (Exception ex)
            {
                RevertChanges();
                return (false, ex.Message);
            }
        }

        private async Task<bool> UpdateFieldAsync(Func<Task<bool>> updateServiceFunction, Action updatePropertyFunction, Func<DoctorModel> updateModelFunction)
        {
            try
            {
                IsLoading = true;
                bool result = await updateServiceFunction();
                if (result)
                {
                    updatePropertyFunction(); 
                    OriginalDoctor = updateModelFunction();
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
    }
}