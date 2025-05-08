using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUI.Model
{
    internal class RecommendationSystemDoctorModel
    {
        public int DoctorId { get; set; }

        public string DoctorName { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public double Rating { get; set; }

        // Tried to refactor to CareerInfo but database won't match
        public string CareerInfo { get; set; }

        public string AvatarUrl { get; set; }

        public string PhoneNumber { get; set; }

        public string Mail { get; set; }

        public static readonly RecommendationSystemDoctorModel Default;

        private const int DEFAULT_DOCTOR_ID = 0;
        private const int DEFAULT_DEPARTMENT_ID = 0;
        private const double DEFAULT_RATING = 0.0;
        private const string DEFAULT_NAME = "Guest";
        private const string DEFAULT_DEPARTMENT_NAME = "Department";
        private const string DEFAULT_AVATAR_URL = "https://picsum.photos/200";
        private const string DEFAULT_CAREER_INFO = "";
        private const string DEFAULT_PHONE = "";
        private const string DEFAULT_EMAIL = "";

        public RecommendationSystemDoctorModel Clone(
            string doctor_name = null,
            int? department_id = null,
            string department_name = null,
            double? rating = null,
            string career_information = null,
            string avatar_url = null,
            string phone_number = null,
            string email = null)
        {
            return new RecommendationSystemDoctorModel(
                this.DoctorId,
                doctor_name ?? this.DoctorName,
                department_id ?? this.DepartmentId,
                department_name ?? this.DepartmentName,
                rating ?? this.Rating,
                career_information ?? this.CareerInfo,
                avatar_url ?? this.AvatarUrl,
                phone_number ?? this.PhoneNumber,
                email ?? this.Mail
            );
        }

        static RecommendationSystemDoctorModel()
        {
            Default = new RecommendationSystemDoctorModel
            {
                DoctorId = DEFAULT_DOCTOR_ID,
                DoctorName = DEFAULT_NAME,
                DepartmentId = DEFAULT_DEPARTMENT_ID,
                DepartmentName = DEFAULT_DEPARTMENT_NAME,
                Rating = DEFAULT_RATING,
                CareerInfo = DEFAULT_CAREER_INFO,
                AvatarUrl = DEFAULT_AVATAR_URL,
                PhoneNumber = DEFAULT_PHONE,
                Mail = DEFAULT_EMAIL,
            };
        }

        public RecommendationSystemDoctorModel() { }

        public RecommendationSystemDoctorModel(
            int doctor_id,
            string doctor_name,
            int department_id,
            string department_name,
            double rating,
            string career_info,
            string avatar_url,
            string phone_number,
            string mail)
        {
            this.DoctorId = doctor_id;
            this.DoctorName = doctor_name;
            this.DepartmentId = department_id;
            this.DepartmentName = department_name;
            this.Rating = rating;
            this.CareerInfo = career_info;
            this.AvatarUrl = string.IsNullOrWhiteSpace(avatar_url) ? DEFAULT_AVATAR_URL : avatar_url;
            this.PhoneNumber = phone_number;
            this.Mail = mail;
        }
    }
}
