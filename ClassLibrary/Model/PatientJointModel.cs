using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Model
{
    public class PatientJointModel
    {
        public int userId { get; set; }
        public int patientId { get; set; }
        public string patientName { get; set; }
        public string bloodType { get; set; }
        public string emergencyContact { get; set; }
        public string allergies { get; set; }
        public double weight { get; set; }
        public int height { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public DateOnly birthDate { get; set; }
        public string cnp { get; set; }
        public string address { get; set; }
        public string phoneNumber { get; set; }
        public DateTime registrationDate { get; set; }

        public static readonly PatientJointModel Default = new(
            -1, -1, string.Empty, string.Empty, string.Empty, string.Empty,
            -1, -1, string.Empty, string.Empty, string.Empty,
            DateOnly.MaxValue, string.Empty, string.Empty, string.Empty, DateTime.Now
        );
        
        public PatientJointModel() { }

        public PatientJointModel(
            int user_id,
            int patient_id,
            string patient_name,
            string blood_type,
            string emergency_contact,
            string allergies,
            double weight,
            int height,
            string username,
            string password,
            string email,
            DateOnly birth_date,
            string cnp,
            string address,
            string phone_number,
            DateTime registration_date
        )
        {
            this.userId = user_id;
            this.patientId = patient_id;
            this.patientName = patient_name;
            this.bloodType = blood_type;
            this.emergencyContact = emergency_contact;
            this.allergies = allergies;
            this.weight = weight;
            this.height = height;
            this.username = username;
            this.password = password;
            this.email = email;
            this.birthDate = birth_date;
            this.cnp = cnp;
            this.address = address;
            this.phoneNumber = phone_number;
            this.registrationDate = registration_date;
        }
    }
}
