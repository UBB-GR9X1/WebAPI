using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUI.Model;

namespace WinUI.ViewModel
{
    public interface IPatientViewModel : INotifyPropertyChanged
    {
        int UserId { get; set; }
        string Name { get; set; }
        string Email { get; set; }
        string Username { get; set; }
        string Address { get; set; }
        string PhoneNumber { get; set; }
        string EmergencyContact { get; set; }
        string BloodType { get; set; }
        string Allergies { get; set; }
        DateTime BirthDate { get; set; }
        string Cnp { get; set; }
        DateTime RegistrationDate { get; set; }
        double Weight { get; set; }
        int Height { get; set; }
        bool IsLoading { get; set; }
        string Password { get; set; }

        PatientJointModel _originalPatient { get; }

        Task<bool> LoadPatientInfoByUserIdAsync(int userId);
        Task<bool> UpdateName(string name);
        Task<bool> UpdateEmail(string email);
        Task<bool> UpdateUsername(string username);
        Task<bool> UpdateAddress(string address);
        Task<bool> UpdatePassword(string password);
        Task<bool> UpdatePhoneNumber(string phoneNumber);
        Task<bool> UpdateEmergencyContact(string emergencyContact);
        Task<bool> UpdateWeight(double weight);
        Task<bool> UpdateHeight(int height);
        
        // Task<bool> LogUpdate(int userId, ActionType action);
    }
}
