using System;
using System.ComponentModel;
using System.Threading.Tasks;
using ClassLibrary.Domain;
using ClassLibrary.IService;

namespace WebClient.Models
{
    public class PatientViewModel
    {
        public int UserId { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Username { get; set; } = "";
        public string Address { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string BloodType { get; set; } = "";
        public string Allergies { get; set; } = "";
        public DateTime BirthDate { get; set; }
        public string CNP { get; set; } = "";
        public DateTime RegistrationDate { get; set; }
        public string EmergencyContact { get; set; } = "";
        public double Weight { get; set; }
        public int Height { get; set; }
        public string Password { get; set; } = "";

        public PatientJointModel OriginalPatient { get; set; } = PatientJointModel.Default;

        public async Task<bool> LoadPatientInfoByUserIdAsync(IPatientService service, int userId)
        {
            var success = await service.loadPatientInfoByUserId(userId);
            var patient = service.patientInfo;

            if (success && patient != PatientJointModel.Default)
            {
                UserId = userId;
                Name = patient.patientName;
                Email = patient.email;
                Username = patient.username;
                Address = patient.address;
                PhoneNumber = patient.phoneNumber;
                EmergencyContact = patient.emergencyContact;
                BloodType = patient.bloodType;
                Allergies = patient.allergies;
                BirthDate = patient.birthDate.ToDateTime(new TimeOnly(0, 0));
                CNP = patient.cnp;
                RegistrationDate = patient.registrationDate;
                Weight = patient.weight;
                Height = patient.height;

                OriginalPatient = patient;
            }

            return success;
        }

        public async Task<bool> UpdateEmergencyContact(IPatientService service)
        {
            if (EmergencyContact != OriginalPatient.emergencyContact)
                return await service.updateEmergencyContact(UserId, EmergencyContact);
            return false;
        }

        public async Task<bool> UpdateWeight(IPatientService service)
        {
            if (Weight != OriginalPatient.weight)
                return await service.updateWeight(UserId, Weight);
            return false;
        }

        public async Task<bool> UpdateHeight(IPatientService service)
        {
            if (Height != OriginalPatient.height)
                return await service.updateHeight(UserId, Height);
            return false;
        }

        public async Task<bool> UpdatePassword(IPatientService service)
        {
            if (!string.IsNullOrWhiteSpace(Password) && Password != OriginalPatient.password)
                return await service.updatePassword(UserId, Password);
            return false;
        }

        public async Task<bool> UpdateName(IPatientService service)
        {
            if (Name != OriginalPatient.patientName)
                return await service.updateName(UserId, Name);
            return false;
        }

        public async Task<bool> UpdateAddress(IPatientService service)
        {
            if (Address != OriginalPatient.address)
                return await service.updateAddress(UserId, Address);
            return false;
        }

        public async Task<bool> UpdatePhoneNumber(IPatientService service)
        {
            if (PhoneNumber != OriginalPatient.phoneNumber)
                return await service.updatePhoneNumber(UserId, PhoneNumber);
            return false;
        }

        public async Task<bool> UpdateBloodType(IPatientService service)
        {
            if (BloodType != OriginalPatient.bloodType)
                return await service.updateBloodType(UserId, BloodType);
            return false;
        }

        public async Task<bool> UpdateAllergies(IPatientService service)
        {
            if (Allergies != OriginalPatient.allergies)
                return await service.updateAllergies(UserId, Allergies);
            return false;
        }
    }
}
