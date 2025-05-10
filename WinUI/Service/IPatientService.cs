using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WinUI.Model;

namespace WinUI.Service
{
    public interface IPatientService
    {
        PatientJointModel patientInfo { get; }
        List<PatientJointModel> patientList { get; }

        Task<bool> loadPatientInfoByUserId(int user_id);
        Task<bool> loadAllPatients();

        Task<bool> updatePassword(int user_id, string password);
        Task<bool> updateName(int user_id, string name);
        Task<bool> updateAddress(int user_id, string _address);
        Task<bool> updatePhoneNumber(int user_id, string phone_number);
        Task<bool> updateWeight(int user_id, double weight);
        Task<bool> updateHeight(int user_id, int height);
        Task<bool> updateEmergencyContact(int user_id, string emergency_contact);
        Task<bool> updateBloodType(int user_id, string blood_type);
        Task<bool> updateAllergies(int user_id, string allergies);
        Task<bool> logUpdate(int user_id, ActionType action);
    }
}
