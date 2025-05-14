using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WinUI.Model
{
    internal class RecommendationSystemDoctorJointModel
    {
        public int doctorId { get; set; }
        public int userId { get; set; }
        public int departmentId { get; set; }
        public double rating { get; set; }
        public string licenseNumber { get; set; }
        public string doctorName { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string mail { get; set; }
        public DateOnly birthDate { get; set; }
        public string cnp { get; set; }
        public string address { get; set; }
        public string phoneNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string departmentName { get; set; }

        public RecommendationSystemDoctorJointModel(int doctorId, int userId, string doctorName, int departmentId, double rating, string licenseNumber, string username, string password, string mail, DateOnly birthDate, string cnp, string address, string phoneNumber, DateTime registrationDate)
        {
            this.doctorId = doctorId;
            this.userId = userId;
            this.departmentId = departmentId;
            this.rating = rating;
            this.licenseNumber = licenseNumber;
            this.doctorName = doctorName;
            this.username = username;
            this.password = password;
            this.mail = mail;
            this.birthDate = birthDate;
            this.cnp = cnp;
            this.address = address;
            this.phoneNumber = phoneNumber;
            RegistrationDate = registrationDate;
        }

        public string getDoctorName()
        {
            Debug.WriteLine($"GetDoctorName called - DoctorName: {doctorName}");
            return doctorName;
        }
        public double getDoctorRating()
        {
            Debug.WriteLine($"GetDoctorRating called - Rating: {rating}");
            return rating;
        }
        public DateOnly getBirthDate()
        {
            return birthDate;
        }
        public DateTime getRegistrationDate()
        {
            return RegistrationDate;
        }
        public string getDoctorDepartment()
        {
            var department = departmentId switch
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
            Debug.WriteLine($"GetDoctorDepartment called - DepartmentId: {departmentId}, Department: {department}");
            return department;
        }
        public string GetDepartmentName()
        {
            return departmentName;
        }
        public override string ToString()
        {
            return $"{doctorName} (Department ID: {departmentId}, Rating: {rating})";
        }

        public double GetRating()
        {
            return rating;
        }
    }
}
