using System;
using System.ComponentModel;
using System.Threading.Tasks;
using WinUI.Model;

namespace WinUI.ViewModel
{
    public interface IPatientViewModel : INotifyPropertyChanged
    {
        int userId { get; set; }
        string name { get; set; }
        string email { get; set; }
        string username { get; set; }
        string password { get; set; }
        string address { get; set; }
        string phone_number { get; set; }
        string emergency_contact { get; set; }
        string blood_type { get; set; }
        string allergies { get; set; }
        DateOnly birth_date { get; set; }
        string cnp { get; set; }
        DateTime registration_date { get; set; }
        double weight { get; set; }
        int height { get; set; }
        bool isLoading { get; set; }

        PatientJointModel original_patient { get; }

        Task<bool> loadPatientInfoByUserIdAsync(int user_id);
        Task<bool> updateEmergencyContact(string emergency_contact);
        Task<bool> updateWeight(double weight);
        Task<bool> updateHeight(int height);
        Task<bool> updatePassword(string password);
        Task<bool> updateName(string name);
        Task<bool> updateAddress(string address);
        Task<bool> updatePhoneNumber(string phoneNumber);
        Task<bool> updateAllergies(string allergies);
        Task<bool> updateBloodType(string bloodType);
        Task<bool> logUpdate(int userId, ActionType action);
    }
}