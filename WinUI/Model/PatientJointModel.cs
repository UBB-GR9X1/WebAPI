using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUI.Model
{
    public class PatientJointModel
    {
        public int UserId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string BloodType { get; set; }
        public string EmergencyContact { get; set; }
        public string Allergies { get; set; }
        public double Weight { get; set; }
        public int Height { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateOnly BirthDate { get; set; }
        public string CNP { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RegistrationDate { get; set; }

        public static readonly PatientJointModel Default = new(
            -1, -1, string.Empty, string.Empty, string.Empty, string.Empty,
            -1, -1, string.Empty, string.Empty, string.Empty,
            DateOnly.MaxValue, string.Empty, string.Empty, string.Empty, DateTime.Now
        );

        public PatientJointModel(
            int userId,
            int patientId,
            string patientName,
            string bloodType,
            string emergencyContact,
            string allergies,
            double weight,
            int height,
            string username,
            string password,
            string email,
            DateOnly birthDate,
            string cnp,
            string address,
            string phoneNumber,
            DateTime registrationDate
        )
        {
            UserId = userId;
            PatientId = patientId;
            PatientName = patientName;
            BloodType = bloodType;
            EmergencyContact = emergencyContact;
            Allergies = allergies;
            Weight = weight;
            Height = height;
            Username = username;
            Password = password;
            Email = email;
            BirthDate = birthDate;
            CNP = cnp;
            Address = address;
            PhoneNumber = phoneNumber;
            RegistrationDate = registrationDate;
        }
    }
}
