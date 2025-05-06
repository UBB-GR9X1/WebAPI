using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.IRepository;
using ClassLibrary.Domain;
using WinUI.Model;
using WinUI.Service;

namespace WinUI.Service
{
    class PatientService : IPatientService
    {
        private readonly IPatientRepository _patient_repository; // ← From ClassLibrary.IRepository

        public PatientJointModel _patient_info { get; private set; } = PatientJointModel.Default;
        public List<PatientJointModel> _patient_list { get; private set; } = new List<PatientJointModel>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientService"/> class with the specified patient repository.
        /// </summary>
        /// <param name="_patient_repository">The patient repository to use for data access.</param>
        public PatientService(IPatientRepository _patient_repository)
        {
            this._patient_repository = _patient_repository;
        }

        /// <summary>
        /// Loads patient information for the specified user ID.
        /// </summary>
        /// <param name="_user_id">The user ID for which to load patient information.</param>
        /// <returns>True if the patient was found and loaded; otherwise, false.</returns>
        public async Task<bool> loadPatientInfoByUserId(int _user_id)
        {
            Patient domain_patient = await this._patient_repository.getPatientByUserIdAsync(_user_id);
            if (domain_patient == null)
            {
                _patient_info = PatientJointModel.Default;
                return false;
            }

            this._patient_info = mapToJointModel(domain_patient);
            Debug.WriteLine($"Patient info loaded: {this._patient_info.patient_name}");
            return true;
        }

        /// <summary>
        /// Loads all patient records into the service.
        /// </summary>
        /// <returns>True if the operation succeeded.</returns>
        public async Task<bool> loadAllPatients()
        {
            List<Patient> domain_patients = await this._patient_repository.getAllPatientsAsync();
            this._patient_list = domain_patients.Select(mapToJointModel).ToList();
            return true;
        }

        public virtual async Task<bool> updatePassword(int _user_id, string _password)
        {
            throw new NotSupportedException("updatePassword is not supported in IPatientRepository from ClassLibrary.");
        }

        public virtual async Task<bool> updateEmail(int _user_id, string _email)
        {
            throw new NotSupportedException("updateEmail is not supported in IPatientRepository from ClassLibrary.");
        }

        public virtual async Task<bool> updateUsername(int _user_id, string _username)
        {
            throw new NotSupportedException("updateUsername is not supported in IPatientRepository from ClassLibrary.");
        }

        public virtual async Task<bool> updateName(int _user_id, string _name)
        {
            throw new NotSupportedException("updateName is not supported in IPatientRepository from ClassLibrary.");
        }

        public virtual async Task<bool> updateBirthDate(int _user_id, DateOnly _birth_date)
        {
            throw new NotSupportedException("updateBirthDate is not supported in IPatientRepository from ClassLibrary.");
        }

        public virtual async Task<bool> updateAddress(int _user_id, string _address)
        {
            throw new NotSupportedException("updateAddress is not supported in IPatientRepository from ClassLibrary.");
        }

        public virtual async Task<bool> updatePhoneNumber(int _user_id, string _phone_number)
        {
            throw new NotSupportedException("updatePhoneNumber is not supported in IPatientRepository from ClassLibrary.");
        }

        /// <summary>
        /// Updates the emergency contact information for the specified user.
        /// </summary>
        /// <param name="_user_id">The user ID whose emergency contact will be updated.</param>
        /// <param name="_emergency_contact">The new emergency contact information.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        public virtual async Task<bool> updateEmergencyContact(int _user_id, string _emergency_contact)
        {
            // This one is supported
            Patient domain_patient = await _patient_repository.getPatientByUserIdAsync(_user_id);
            if (domain_patient == null) return false;
            domain_patient.emergencyContact = _emergency_contact;
            await this._patient_repository.addPatientAsync(domain_patient);
            return true;
        }

        /// <summary>
        /// Updates the weight for the specified user.
        /// </summary>
        /// <param name="_user_id">The user ID whose weight will be updated.</param>
        /// <param name="weight">The new weight value.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        public virtual async Task<bool> updateWeight(int _user_id, double weight)
        {
            Patient domain_patient = await this._patient_repository.getPatientByUserIdAsync(_user_id);
            if (domain_patient == null) return false;
            domain_patient.weight = weight;
            await this._patient_repository.addPatientAsync(domain_patient);
            return true;
        }

        /// <summary>
        /// Updates the height for the specified user.
        /// </summary>
        /// <param name="_user_id">The user ID whose height will be updated.</param>
        /// <param name="height">The new height value.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        public virtual async Task<bool> updateHeight(int _user_id, int height)
        {
            Patient domain_patient = await this._patient_repository.getPatientByUserIdAsync(_user_id);
            if (domain_patient == null) return false;
            domain_patient.height = height;
            await this._patient_repository.addPatientAsync(domain_patient); // ← assumes Add is Upsert
            return true;
        }

        public virtual async Task<bool> logUpdate(int _user_id, ActionType action)
        {
            Debug.WriteLine($"logUpdate called for user {_user_id}, action: {action}");
            return await Task.FromResult(true); // stub implementation
        }

        private PatientJointModel mapToJointModel(Patient _domain_patient)
        {
            return new PatientJointModel(
                _domain_patient.userId,
                _domain_patient.userId, // using _user_id as patient_id if not available
                $"Patient {_domain_patient.userId}", // placeholder
                _domain_patient.bloodType ?? string.Empty,
                _domain_patient.emergencyContact ?? string.Empty,
                _domain_patient.allergies ?? string.Empty,
                _domain_patient.weight,
                _domain_patient.height,
                $"user{_domain_patient.userId}", // placeholder
                string.Empty, // placeholder
                $"patient{_domain_patient.userId}@example.com", // placeholder
                DateOnly.FromDateTime(DateTime.Now), // placeholder
                string.Empty, // placeholder
                string.Empty, // placeholder
                string.Empty, // placeholder
                DateTime.Now // placeholder
            );
        }
    }
}
