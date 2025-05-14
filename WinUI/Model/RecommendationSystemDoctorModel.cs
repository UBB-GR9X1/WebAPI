using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUI.Model
{
    internal class RecommendationSystemDoctorModel
    {
        public int doctorId { get; set; }

        public string doctorName { get; set; }

        public int departmentId { get; set; }

        public string departmentName { get; set; }

        public double rating { get; set; }

        // Tried to refactor to CareerInfo but database won't match
        public string careerInfo { get; set; }

        public string avatarUrl { get; set; }

        public string phoneNumber { get; set; }

        public string mail { get; set; }

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
                this.doctorId,
                doctor_name ?? this.doctorName,
                department_id ?? this.departmentId,
                department_name ?? this.departmentName,
                rating ?? this.rating,
                career_information ?? this.careerInfo,
                avatar_url ?? this.avatarUrl,
                phone_number ?? this.phoneNumber,
                email ?? this.mail
            );
        }

        static RecommendationSystemDoctorModel()
        {
            Default = new RecommendationSystemDoctorModel
            {
                doctorId = DEFAULT_DOCTOR_ID,
                doctorName = DEFAULT_NAME,
                departmentId = DEFAULT_DEPARTMENT_ID,
                departmentName = DEFAULT_DEPARTMENT_NAME,
                rating = DEFAULT_RATING,
                careerInfo = DEFAULT_CAREER_INFO,
                avatarUrl = DEFAULT_AVATAR_URL,
                phoneNumber = DEFAULT_PHONE,
                mail = DEFAULT_EMAIL,
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
            this.doctorId = doctor_id;
            this.doctorName = doctor_name;
            this.departmentId = department_id;
            this.departmentName = department_name;
            this.rating = rating;
            this.careerInfo = career_info;
            this.avatarUrl = string.IsNullOrWhiteSpace(avatar_url) ? DEFAULT_AVATAR_URL : avatar_url;
            this.phoneNumber = phone_number;
            this.mail = mail;
        }
    }
}
