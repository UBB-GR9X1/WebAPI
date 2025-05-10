using System;
using System.ComponentModel;
using System.Threading.Tasks;
using WinUI.Model;
using WinUI.Service;

namespace WinUI.ViewModel
{
    public class PatientViewModel
    {
        private readonly IPatientService _patient_service;
        public event PropertyChangedEventHandler? property_changed;
        public string password { get; set; } = string.Empty;
        private int _user_id;
        private string _name = string.Empty;
        private string _email = string.Empty;
        private string _username = string.Empty;
        private string _address = string.Empty;
        private string _phone_number = string.Empty;
        private string _blood_type = string.Empty;
        private string _allergies = string.Empty;
        private DateOnly _birth_date;
        private string _cnp = string.Empty;
        private DateTime _registration_date;
        private string _emergency_contact = string.Empty;
        private double _weight;
        private bool _is_loading;
        private int _height;

        public PatientJointModel original_patient { get; private set; }

        public PatientViewModel(IPatientService patient_service, int user_id)
        {
            this._patient_service = patient_service;
            this._user_id = user_id;
            this.original_patient = PatientJointModel.Default;
            _ = loadPatientInfoByUserIdAsync(_user_id);
        }

        public int userId
        {
            get => this._user_id;
            set { if (this._user_id != value) { this._user_id = value; OnPropertyChanged(); } }
        }

        public string name
        {
            get => this._name;
            set { if (this._name != value) { this._name = value; OnPropertyChanged(); } }
        }

        public string email
        {
            get => this._email;
            set { if (this._email != value) { this._email = value; OnPropertyChanged(); } }
        }

        public string username
        {
            get => this._username;
            set { if (this._username != value) { this._username = value; OnPropertyChanged(); } }
        }

        public string address
        {
            get => this._address;
            set { if (this._address != value) { this._address = value; OnPropertyChanged(); } }
        }

        public string phoneNumber
        {
            get => this._phone_number;
            set { if (this._phone_number != value) { this._phone_number = value; OnPropertyChanged(); } }
        }

        public string bloodType
        {
            get => this._blood_type;
            set { if (this._blood_type != value) { this._blood_type = value; OnPropertyChanged(); } }
        }

        public string allergies
        {
            get => this._allergies;
            set { if (this._allergies != value) { this._allergies = value; OnPropertyChanged(); } }
        }

        public DateOnly birthDate
        {
            get => this._birth_date;
            set { if (this._birth_date != value) { this._birth_date = value; OnPropertyChanged(); } }
        }

        public string cnp
        {
            get => this._cnp;
            set { if (this._cnp != value) { this._cnp = value; OnPropertyChanged(); } }
        }

        public DateTime registrationDate
        {
            get => this._registration_date;
            set { if (this._registration_date != value) { this._registration_date = value; OnPropertyChanged(); } }
        }

        public string emergencyContact
        {
            get => this._emergency_contact;
            set { if (this._emergency_contact != value) { this._emergency_contact = value; OnPropertyChanged(); } }
        }

        public double weight
        {
            get => this._weight;
            set { if (this._weight != value) { this._weight = value; OnPropertyChanged(); } }
        }

        public int height
        {
            get => this._height;
            set { if (this._height != value) { this._height = value; OnPropertyChanged(); } }
        }

        public bool isLoading
        {
            get => this._is_loading;
            set { if (this._is_loading != value) { this._is_loading = value; OnPropertyChanged(); } }
        }

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            property_changed?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<bool> loadPatientInfoByUserIdAsync(int user_id)
        {
            try
            {
                this.isLoading = true;

                bool success = await this._patient_service.loadPatientInfoByUserId(user_id);
                PatientJointModel? patient = this._patient_service.patientInfo;

                if (success && patient != PatientJointModel.Default)
                {
                    this.name = patient.patientName;
                    this.email = patient.email;
                    this.username = patient.username;
                    this.address = patient.address;
                    this.phoneNumber = patient.phoneNumber;
                    this.emergencyContact = patient.emergencyContact;
                    this.bloodType = patient.bloodType;
                    this.allergies = patient.allergies;
                    this.birthDate = patient.birthDate;
                    this.cnp = patient.cnp;
                    this.registrationDate = patient.registrationDate;
                    this.weight = patient.weight;
                    this.height = patient.height;

                    this.original_patient = new PatientJointModel(
                        this._user_id,
                        patient.patientId,
                        this.name,
                        this.bloodType,
                        this.emergencyContact,
                        this.allergies,
                        this.weight,
                        this.height,
                        this.username,
                        "", // password unused now
                        this.email,
                        patient.birthDate,
                        this.cnp,
                        this.address,
                        this.phoneNumber,
                        this.registrationDate
                    );
                }

                this.isLoading = false;
                return success;
            }
            catch (Exception exception)
            {
                this.isLoading = false;
                Console.WriteLine($"Error loading patient info: {exception.Message}");
                return false;
            }
        }

        public async Task<bool> updateEmergencyContact(string emergency_contact)
        {
            try
            {
                this.isLoading = true;
                bool updated = await this._patient_service.updateEmergencyContact(userId, emergency_contact);
                if (updated)
                {
                    this.emergencyContact = emergency_contact;
                    this.original_patient.emergencyContact = emergency_contact;
                }
                return updated;
            }
            finally { this.isLoading = false; }
        }

        public async Task<bool> updateWeight(double weight)
        {
            try
            {
                this.isLoading = true;
                bool updated = await this._patient_service.updateWeight(userId, weight);
                if (updated)
                {
                    this.weight = weight;
                    this.original_patient.weight = this.weight;
                }
                return updated;
            }
            finally { this.isLoading = false; }
        }

        public async Task<bool> updateHeight(int height)
        {
            try
            {
                this.isLoading = true;
                bool updated = await this._patient_service.updateHeight(userId, height);
                if (updated)
                {
                    this.height = height;
                    this.original_patient.height = height;
                }
                return updated;
            }
            finally { this.isLoading = false; }
        }

        public async Task<bool> updatePassword(string password)
        {
            try
            {
                this.isLoading = true;
                bool updated = await this._patient_service.updatePassword(userId, password);
                if (updated)
                {
                    this.password = password;
                    this.original_patient.password = password;
                }
                return updated;
            }
            finally { this.isLoading = false; }
        }

        public async Task<bool> updateName(string name)
        {
            try
            {
                this.isLoading = true;
                bool updated = await this._patient_service.updateName(userId, name);
                if (updated)
                {
                    this.name = name;
                    this.original_patient.patientName = name;
                }
                return updated;
            }
            finally { this.isLoading = false; }
        }

        public async Task<bool> updateAddress(string address)
        {
            try
            {
                this.isLoading = true;
                bool updated = await this._patient_service.updateAddress(userId, address);
                if (updated)
                {
                    this.address = address;
                    this.original_patient.address = address;
                }
                return updated;
            }
            finally { this.isLoading = false; }
        }

        public async Task<bool> updatePhoneNumber(string phone_number)
        {
            try
            {
                this.isLoading = true;
                bool updated = await this._patient_service.updatePhoneNumber(userId, phone_number);
                if (updated)
                {
                    this.phoneNumber = phone_number;
                    this.original_patient.phoneNumber = phone_number;
                }
                return updated;
            }
            finally { this.isLoading = false; }
        }

        public async Task<bool> updateBloodType(string blood_type)
        {
            try
            {
                this.isLoading = true;
                bool updated = await this._patient_service.updateBloodType(userId, blood_type);
                if (updated)
                {
                    this.bloodType = blood_type;
                    this.original_patient.bloodType = blood_type;
                }
                return updated;
            }
            finally { this.isLoading = false; }
        }

        public async Task<bool> updateAllergies(string allergies)
        {
            try
            {
                this.isLoading = true;
                bool updated = await this._patient_service.updateAllergies(userId, allergies);
                if (updated)
                {
                    this.allergies = allergies;
                    this.original_patient.allergies = allergies;
                }
                return updated;
            }
            finally { this.isLoading = false; }
        }

        public async Task<bool> logUpdate(int user_id, ActionType action)
        {
            return await this._patient_service.logUpdate(user_id, action);
        }
    }
}
