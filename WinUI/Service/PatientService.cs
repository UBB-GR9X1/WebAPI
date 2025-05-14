using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUI.Repository;
using ClassLibrary.Domain;
using WinUI.Model;
using WinUI.Service;
using static WinUI.Proxy.LogInProxy;
using Windows.System;
using User = ClassLibrary.Domain.User;
using System.Net;

namespace WinUI.Service
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patient_repository; // ← From ClassLibrary.IRepository

        public PatientJointModel patientInfo { get; private set; } = PatientJointModel.Default;
        public List<PatientJointModel> patientList { get; private set; } = new List<PatientJointModel>();

        public PatientService(IPatientRepository patient_repository)
        {
            this._patient_repository = patient_repository;
        }

        public async Task<bool> loadPatientInfoByUserId(int user_id)
        {
            Patient domain_patient = await this._patient_repository.getPatientByUserIdAsync(user_id);
            List<User> domain_users = await this._patient_repository.getAllUserAsync();
            User filtered_user = domain_users.Find(user => user.userId == user_id);
            if (domain_patient == null || filtered_user == null)
            {
                patientInfo = PatientJointModel.Default;
                return false;
            }

            this.patientInfo = mapToJointModel(domain_patient, filtered_user);
            Debug.WriteLine($"Patient info loaded: {this.patientInfo.patientName}");
            return true;
        }

        public async Task<bool> loadAllPatients()
        {
            List<Patient> domain_patients = await this._patient_repository.getAllPatientsAsync();
            List<User> domain_users = await this._patient_repository.getAllUserAsync();

            this.patientList.Clear();
            foreach (var patient in domain_patients)
            {
                User? matched_user = domain_users.Find(user => user.userId == patient.userId);
                if (matched_user != null)
                {
                    this.patientList.Add(mapToJointModel(patient, matched_user));
                }
            }
            return true;
        }

        public virtual async Task<bool> updatePassword(int _user_id, string _password)
        {
            Patient domain_patient = await _patient_repository.getPatientByUserIdAsync(_user_id);
            List<User> domain_users = await this._patient_repository.getAllUserAsync();
            User filtered_user = domain_users.Find(user => user.userId == _user_id);
            if (domain_patient == null || filtered_user == null)
            {
                patientInfo = PatientJointModel.Default;
                return false;
            }

            filtered_user.password = _password;
            await this._patient_repository.updatePatientAsync(domain_patient, filtered_user);
            return true;
        }

        public virtual async Task<bool> updateName(int user_id, string name)
        {
            Patient domain_patient = await _patient_repository.getPatientByUserIdAsync(user_id);
            List<User> domain_users = await this._patient_repository.getAllUserAsync();
            User filtered_user = domain_users.Find(user => user.userId == user_id);
            if (domain_patient == null || filtered_user == null)
            {
                patientInfo = PatientJointModel.Default;
                return false;
            }

            filtered_user.name = name;
            await this._patient_repository.updatePatientAsync(domain_patient, filtered_user);
            return true;
        }

        public virtual async Task<bool> updateAddress(int user_id, string address)
        {
            Patient domain_patient = await _patient_repository.getPatientByUserIdAsync(user_id);
            List<User> domain_users = await this._patient_repository.getAllUserAsync();
            User filtered_user = domain_users.Find(user => user.userId == user_id);
            if (domain_patient == null || filtered_user == null)
            {
                patientInfo = PatientJointModel.Default;
                return false;
            }

            filtered_user.address = address;
            await this._patient_repository.updatePatientAsync(domain_patient, filtered_user);
            return true;
        }

        public virtual async Task<bool> updatePhoneNumber(int user_id, string phone_number)
        {
            Patient domain_patient = await _patient_repository.getPatientByUserIdAsync(user_id);
            List<User> domain_users = await this._patient_repository.getAllUserAsync();
            User filtered_user = domain_users.Find(user => user.userId == user_id);
            if (domain_patient == null || filtered_user == null)
            {
                patientInfo = PatientJointModel.Default;
                return false;
            }

            filtered_user.phoneNumber = phone_number;
            await this._patient_repository.updatePatientAsync(domain_patient, filtered_user);
            return true;
        }

        public virtual async Task<bool> updateEmergencyContact(int user_id, string emergency_contact)
        {
            Patient domain_patient = await _patient_repository.getPatientByUserIdAsync(user_id);
            if (domain_patient == null) return false;
            domain_patient.EmergencyContact = emergency_contact;
            await this._patient_repository.addPatientAsync(domain_patient);
            return true;
        }

        public virtual async Task<bool> updateWeight(int user_id, double weight)
        {
            Patient domain_patient = await this._patient_repository.getPatientByUserIdAsync(user_id);
            if (domain_patient == null) return false;
            domain_patient.weight = weight;
            await this._patient_repository.addPatientAsync(domain_patient);
            return true;
        }

        public virtual async Task<bool> updateHeight(int user_id, int height)
        {
            Patient domain_patient = await this._patient_repository.getPatientByUserIdAsync(user_id);
            if (domain_patient == null) return false;
            domain_patient.height = height;
            await this._patient_repository.addPatientAsync(domain_patient);
            return true;
        }

        public virtual async Task<bool> logUpdate(int user_id, ActionType action)
        {
            Debug.WriteLine($"logUpdate called for user {user_id}, action: {action}");
            return await Task.FromResult(true);
        }

        private PatientJointModel mapToJointModel(Patient domain_patient, User domain_user)
        {
            return new PatientJointModel(
                domain_patient.userId,
                domain_user.userId,
                domain_user.name,
                domain_patient.bloodType ?? string.Empty,
                domain_patient.EmergencyContact ?? string.Empty,
                domain_patient.allergies ?? string.Empty,
                domain_patient.weight,
                domain_patient.height,
                domain_user.username,
                domain_user.password,
                domain_user.mail,
                domain_user.birthDate,
                domain_user.cnp ?? string.Empty,
                domain_user.address ?? string.Empty,
                domain_user.phoneNumber ?? string.Empty,
                domain_user.registrationDate
            );
        }
    }
}
