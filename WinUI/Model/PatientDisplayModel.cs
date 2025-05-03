using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUI.Model
{
    public class PatientDisplayModel
    {
        public int PatientId { get; set; }
        public string FullName { get; set; }
        public string CNP { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string BloodType { get; set; }
        public string EmergencyContactName { get; set; }
        public string Allergies { get; set; }
        public float Weight { get; set; }
        public int Height { get; set; }
        public DateTime RegistrationDate { get; set; }

        public PatientDisplayModel(
            int patientId,
            string fullName,
            string cnp,
            string username,
            string email,
            string password,
            DateOnly birthDate,
            string address,
            string phoneNumber,
            string bloodType,
            string emergencyContactName,
            string allergies,
            float weight,
            int height,
            DateTime registrationDate
        )
        {
            PatientId = patientId;
            FullName = fullName;
            CNP = cnp;
            Username = username;
            Email = email;
            Password = password;
            BirthDate = birthDate;
            Address = address;
            PhoneNumber = phoneNumber;
            BloodType = bloodType;
            EmergencyContactName = emergencyContactName;
            Allergies = allergies;
            Weight = weight;
            Height = height;
            RegistrationDate = registrationDate;
        }
    }
}
