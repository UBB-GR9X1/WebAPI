using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUI.Model;
using WinUI.Repository;
using WinUI.Service;

namespace WinUI.ViewModel
{
    public class PatientViewModel : IPatientViewModel
    {
        private readonly IPatientService _patientManager;
        public event PropertyChangedEventHandler? PropertyChanged;

        public PatientJointModel _originalPatient { get; private set; }

        public PatientViewModel(IPatientService patientManager, int userId)
        {
            _patientManager = patientManager;
            _userId = userId;
            _originalPatient = PatientJointModel.Default;
            _ = LoadPatientInfoByUserIdAsync(userId);
        }

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

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _username = string.Empty;
        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _address = string.Empty;
        public string Address
        {
            get => _address;
            set
            {
                if (_address != value)
                {
                    _address = value;
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

        private string _emergencyContact = string.Empty;
        public string EmergencyContact
        {
            get => _emergencyContact;
            set
            {
                if (_emergencyContact != value)
                {
                    _emergencyContact = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _bloodType = string.Empty;
        public string BloodType
        {
            get => _bloodType;
            set
            {
                if (_bloodType != value)
                {
                    _bloodType = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _allergies = string.Empty;
        public string Allergies
        {
            get => _allergies;
            set
            {
                if (_allergies != value)
                {
                    _allergies = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _birthDate;
        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                if (_birthDate != value)
                {
                    _birthDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _personalIdentificationNumber = string.Empty;
        public string Cnp
        {
            get => _personalIdentificationNumber;
            set
            {
                if (_personalIdentificationNumber != value)
                {
                    _personalIdentificationNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _registrationDate;
        public DateTime RegistrationDate
        {
            get => _registrationDate;
            set
            {
                if (_registrationDate != value)
                {
                    _registrationDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _weight;
        public double Weight
        {
            get => _weight;
            set
            {
                if (_weight != value)
                {
                    _weight = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _height;
        public int Height
        {
            get => _height;
            set
            {
                if (_height != value)
                {
                    _height = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<bool> LoadPatientInfoByUserIdAsync(int userId)
        {
            try
            {
                IsLoading = true;

                bool success = await _patientManager.LoadPatientInfoByUserId(userId);
                var patient = _patientManager._patientInfo;

                if (success && patient != PatientJointModel.Default)
                {
                    Name = patient.PatientName;
                    Email = patient.Email;
                    Password = patient.Password;
                    Username = patient.Username;
                    Address = patient.Address;
                    PhoneNumber = patient.PhoneNumber;
                    EmergencyContact = patient.EmergencyContact;
                    BloodType = patient.BloodType;
                    Allergies = patient.Allergies;
                    BirthDate = patient.BirthDate.ToDateTime(TimeOnly.MinValue);
                    Cnp = patient.CNP;
                    RegistrationDate = patient.RegistrationDate;
                    Weight = patient.Weight;
                    Height = patient.Height;

                    _originalPatient = new PatientJointModel(
                        userId,
                        patient.PatientId,
                        Name,
                        BloodType,
                        EmergencyContact,
                        Allergies,
                        Weight,
                        Height,
                        Username,
                        Password,
                        Email,
                        patient.BirthDate,
                        Cnp,
                        Address,
                        PhoneNumber,
                        RegistrationDate
                    );
                }

                IsLoading = false;
                return success;
            }
            catch (Exception exception)
            {
                IsLoading = false;
                Console.WriteLine($"Error loading patient info: {exception.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateName(string name)
        {
            try
            {
                IsLoading = true;
                bool updated = await _patientManager.UpdateName(UserId, name);
                if (updated)
                {
                    Name = name;
                    _originalPatient.PatientName = name;
                }
                return updated;
            }
            finally { IsLoading = false; }
        }

        public async Task<bool> UpdateEmail(string email)
        {
            try
            {
                IsLoading = true;
                bool updated = await _patientManager.UpdateEmail(UserId, email);
                if (updated)
                {
                    Email = email;
                    _originalPatient.Email = email;
                }
                return updated;
            }
            finally { IsLoading = false; }
        }

        public async Task<bool> UpdateUsername(string username)
        {
            try
            {
                IsLoading = true;
                bool updated = await _patientManager.UpdateUsername(UserId, username);
                if (updated)
                {
                    Username = username;
                    _originalPatient.Username = username;
                }
                return updated;
            }
            finally { IsLoading = false; }
        }

        public async Task<bool> UpdateAddress(string address)
        {
            try
            {
                IsLoading = true;
                bool updated = await _patientManager.UpdateAddress(UserId, address);
                if (updated)
                {
                    Address = address;
                    _originalPatient.Address = address;
                }
                return updated;
            }
            finally { IsLoading = false; }
        }

        public async Task<bool> UpdatePassword(string password)
        {
            try
            {
                IsLoading = true;
                bool updated = await _patientManager.UpdatePassword(UserId, password);
                if (updated)
                {
                    Password = password;
                    _originalPatient.Password = password;
                }
                return updated;
            }
            finally { IsLoading = false; }
        }

        public async Task<bool> UpdatePhoneNumber(string phoneNumber)
        {
            try
            {
                IsLoading = true;
                bool updated = await _patientManager.UpdatePhoneNumber(UserId, phoneNumber);
                if (updated)
                {
                    PhoneNumber = phoneNumber;
                    _originalPatient.PhoneNumber = phoneNumber;
                }
                return updated;
            }
            finally { IsLoading = false; }
        }

        public async Task<bool> UpdateEmergencyContact(string emergencyContact)
        {
            try
            {
                IsLoading = true;
                bool updated = await _patientManager.UpdateEmergencyContact(UserId, emergencyContact);
                if (updated)
                {
                    EmergencyContact = emergencyContact;
                    _originalPatient.EmergencyContact = emergencyContact;
                }
                return updated;
            }
            finally { IsLoading = false; }
        }

        public async Task<bool> UpdateWeight(double weight)
        {
            try
            {
                IsLoading = true;
                bool updated = await _patientManager.UpdateWeight(UserId, weight);
                if (updated)
                {
                    Weight = weight;
                    _originalPatient.Weight = weight;
                }
                return updated;
            }
            finally { IsLoading = false; }
        }

        public async Task<bool> UpdateHeight(int height)
        {
            try
            {
                IsLoading = true;
                bool updated = await _patientManager.UpdateHeight(UserId, height);
                if (updated)
                {
                    Height = height;
                    _originalPatient.Height = height;
                }
                return updated;
            }
            finally { IsLoading = false; }
        }

        //public async Task<bool> LogUpdate(int userId, ActionType action)
        //{
        //    return await _patientManager.LogUpdate(userId, action);
        //}
    }
}
