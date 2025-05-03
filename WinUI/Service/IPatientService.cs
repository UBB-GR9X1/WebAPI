using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUI.Model;

namespace WinUI.Service
{
    public interface IPatientService
    {
        PatientJointModel _patientInfo { get; }
        List<PatientJointModel> _patientList { get; }

        Task<bool> LoadPatientInfoByUserId(int userId);
        Task<bool> LoadAllPatients();
        Task<bool> UpdatePassword(int userId, string password);
        Task<bool> UpdateEmail(int userId, string email);
        Task<bool> UpdateUsername(int userId, string username);
        Task<bool> UpdateName(int userId, string name);
        Task<bool> UpdateBirthDate(int userId, DateOnly birthDate);
        Task<bool> UpdateAddress(int userId, string address);
        Task<bool> UpdatePhoneNumber(int userId, string phoneNumber);
        Task<bool> UpdateEmergencyContact(int userId, string emergencyContact);
        Task<bool> UpdateWeight(int userId, double weight);
        Task<bool> UpdateHeight(int userId, int height);

        // Task<bool> LogUpdate(int userId, ActionType action);
    }
}
