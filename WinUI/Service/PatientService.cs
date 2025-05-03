using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUI.Model;
using WinUI.Repository;

namespace WinUI.Service
{
    class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        //Use this for working on a specific patient
        public PatientJointModel _patientInfo { get; private set; } = PatientJointModel.Default;

        //Use this for working with more patients
        public List<PatientJointModel> _patientList { get; private set; } = new List<PatientJointModel>();

        // public PatientService() : this(new PatientRepository()) { }

        // Second constructor for test injection
        public PatientService(IPatientRepository testService)
        {
            _patientRepository = testService;
        }


        public async Task<bool> LoadPatientInfoByUserId(int userId)
        {
            _patientInfo = await _patientRepository.GetPatientByUserId(userId).ConfigureAwait(false);
            Debug.WriteLine($"Patient info loaded: {_patientInfo.PatientName}");
            return true;
        }

        public async Task<bool> LoadAllPatients()
        {
            _patientList = await _patientRepository.GetAllPatients().ConfigureAwait(false);
            return true;
        }

        public virtual async Task<bool> UpdatePassword(int userId, string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Contains(' '))
                throw new Exception("Invalid password!\nPassword cannot be empty or contain spaces.");

            if (password.Length > 255)
                throw new Exception("Invalid password!\nPassword cannot exceed 255 characters.");

            return await _patientRepository.UpdatePassword(userId, password);
        }

        public virtual async Task<bool> UpdateEmail(int userId, string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@") || !email.Contains("."))
                throw new Exception("Invalid email format.");

            if (email.Length > 100)
                throw new Exception("Invalid email!\nEmail cannot exceed 100 characters.");

            return await _patientRepository.UpdateEmail(userId, email);
        }

        public virtual async Task<bool> UpdateUsername(int userId, string username)
        {
            if (string.IsNullOrWhiteSpace(username) || username.Contains(' '))
                throw new Exception("Invalid username!\nUsername cannot be empty or contain spaces.");

            if (username.Length > 50)
                throw new Exception("Invalid username!\nUsername cannot exceed 50 characters.");

            return await _patientRepository.UpdateUsername(userId, username);
        }

        public virtual async Task<bool> UpdateName(int userId, string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Any(char.IsDigit))
                throw new Exception("Name cannot be empty or contain digits.");

            if (name.Length > 100)
                throw new Exception("Invalid name!\nName cannot exceed 100 characters.");

            return await _patientRepository.UpdateName(userId, name);
        }

        public virtual async Task<bool> UpdateBirthDate(int userId, DateOnly birthDate)
        {
            return await _patientRepository.UpdateBirthDate(userId, birthDate);
        }

        public virtual async Task<bool> UpdateAddress(int userId, string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                address = "";

            if (address.Length > 255)
                throw new Exception("Invalid address!\nAddress cannot exceed 255 characters.");

            return await _patientRepository.UpdateAddress(userId, address);
        }

        public virtual async Task<bool> UpdatePhoneNumber(int userId, string phoneNumber)
        {
            if (phoneNumber.Length != 10)
                throw new Exception("Invalid phone number!\nPhone number must be 10 digits long.");

            if (!phoneNumber.All(char.IsDigit))
                throw new Exception("Invalid phone number!\nOnly digits are allowed.");

            return await _patientRepository.UpdatePhoneNumber(userId, phoneNumber);
        }

        public virtual async Task<bool> UpdateEmergencyContact(int userId, string emergencyContact)
        {
            if (emergencyContact.Length != 10)
                throw new Exception("Invalid emergency contact!\nContact number must be 10 digits long.");

            if (!emergencyContact.All(char.IsDigit))
                throw new Exception("Invalid emergency contact!\nOnly digits are allowed.");

            return await _patientRepository.UpdateEmergencyContact(userId, emergencyContact);
        }

        public virtual async Task<bool> UpdateWeight(int userId, double weight)
        {
            if (weight <= 0)
                throw new Exception("Invalid weight!\nWeight must be greater than 0.");

            return await _patientRepository.UpdateWeight(userId, weight);
        }

        public virtual async Task<bool> UpdateHeight(int userId, int height)
        {
            if (height <= 0)
                throw new Exception("Invalid height!\nHeight must be greater than 0.");

            return await _patientRepository.UpdateHeight(userId, height);
        }

        //public virtual async Task<bool> LogUpdate(int userId, ActionType action)
        //{
        //    return await _patientRepository.LogUpdate(userId, action);
        //}
    }
}
