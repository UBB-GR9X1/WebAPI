using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUI.Model
{
    internal class RecommendationSystemDoctorJointModel
    {
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
        public double Rating { get; set; }
        public string LicenseNumber { get; set; }
        public string DoctorName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Cnp { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RegistrationDate { get; set; }

        public RecommendationSystemDoctorJointModel(int doctorId, int userId, string doctorName, int departmentId, double rating, string licenseNumber, string username, string password, string mail, DateOnly birthDate, string cnp, string address, string phoneNumber, DateTime registrationDate)
        {
            DoctorId = doctorId;
            UserId = userId;
            DepartmentId = departmentId;
            Rating = rating;
            LicenseNumber = licenseNumber;
            DoctorName = doctorName;
            Username = username;
            Password = password;
            Mail = mail;
            BirthDate = birthDate;
            Cnp = cnp;
            Address = address;
            PhoneNumber = phoneNumber;
            RegistrationDate = registrationDate;
        }

        public string GetDoctorName()
        {
            return DoctorName;
        }
        public double GetDoctorRating()
        {
            return Rating;
        }
        public DateOnly GetBirthDate()
        {
            return BirthDate;
        }
        public DateTime GetRegistrationDate()
        {
            return RegistrationDate;
        }
        public string GetDoctorDepartment()
        {
            return DepartmentId switch
            {
                1 => "Cardiology",
                2 => "Neurology",
                3 => "Pediatrics",
                4 => "Ophthalmology",
                5 => "Gastroenterology",
                6 => "Orthopedics",
                7 => "Dermatology",
                _ => "Unknown"
            };
        }
        public override string ToString()
        {
            return $"{DoctorName} (Department ID: {DepartmentId}, Rating: {Rating})";
        }
    }
}
