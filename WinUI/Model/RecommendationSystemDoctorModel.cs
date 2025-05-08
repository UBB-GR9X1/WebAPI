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

        public const int DefaultDoctorId = 0;
        public const int DefaultDepartmentId = 0;
        public const double DefaultRating = 0.0;
        public const string DefaultName = "Guest";
        public const string DefaultDepartmentName = "Department";
        public const string DefaultAvatarUrl = "https://picsum.photos/200";
        public const string DefaultCareerInfo = "";
        public const string DefaultPhone = "";
        public const string DefaultEmail = "";

        public RecommendationSystemDoctorModel Clone(
            string doctorName = null,
            int? departmentId = null,
            string departmentName = null,
            double? rating = null,
            string careerInformation = null,
            string avatarUrl = null,
            string phoneNumber = null,
            string email = null)
        {
            return new RecommendationSystemDoctorModel(
                DoctorId,
                doctorName ?? DoctorName,
                departmentId ?? DepartmentId,
                departmentName ?? DepartmentName,
                rating ?? Rating,
                careerInformation ?? CareerInfo,
                avatarUrl ?? AvatarUrl,
                phoneNumber ?? PhoneNumber,
                email ?? Mail
            );
        }

        static RecommendationSystemDoctorModel()
        {
            Default = new RecommendationSystemDoctorModel
            {
                DoctorId = DefaultDoctorId,
                DoctorName = DefaultName,
                DepartmentId = DefaultDepartmentId,
                DepartmentName = DefaultDepartmentName,
                Rating = DefaultRating,
                CareerInfo = DefaultCareerInfo,
                AvatarUrl = DefaultAvatarUrl,
                PhoneNumber = DefaultPhone,
                Mail = DefaultEmail,
            };
        }

        public RecommendationSystemDoctorModel() { }

        public RecommendationSystemDoctorModel(
            int doctorId,
            string doctorName,
            int departmentId,
            string departmentName,
            double rating,
            string careerInfo,
            string avatarUrl,
            string phoneNumber,
            string mail)
        {
            DoctorId = doctorId;
            DoctorName = doctorName;
            DepartmentId = departmentId;
            DepartmentName = departmentName;
            Rating = rating;
            CareerInfo = careerInfo;
            AvatarUrl = string.IsNullOrWhiteSpace(avatarUrl) ? DefaultAvatarUrl : avatarUrl;
            PhoneNumber = phoneNumber;
            Mail = mail;
        }
    }
}
