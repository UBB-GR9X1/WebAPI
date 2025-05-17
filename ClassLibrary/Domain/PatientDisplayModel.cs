using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Domain
{
    public class PatientDisplayModel
    {
        public int patientId { get; set; }
        public string fullName { get; set; }
        public string cnp { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public DateOnly birthDate { get; set; }
        public string address { get; set; }
        public string phoneNumber { get; set; }
        public string bloodType { get; set; }
        public string emergencyContactName { get; set; }
        public string allergies { get; set; }
        public float weight { get; set; }
        public int height { get; set; }
        public DateTime registrationDate { get; set; }

        public PatientDisplayModel(
            int patient_id,
            string fullname,
            string cnp,
            string username,
            string email,
            string password,
            DateOnly birth_date,
            string address,
            string phone_number,
            string blood_type,
            string emergency_contact_name,
            string allergies,
            float weight,
            int height,
            DateTime registration_date
        )
        {
            this.patientId = patient_id;
            this.fullName = fullname;
            this.cnp = cnp;
            this.username = username;
            this.email = email;
            this.password = password;
            this.birthDate = birth_date;
            this.address = address;
            this.phoneNumber = phone_number;
            this.bloodType = blood_type;
            this.emergencyContactName = emergency_contact_name;
            this.allergies = allergies;
            this.weight = weight;
            this.height = height;
            this.registrationDate = registration_date;
        }
    }
}
