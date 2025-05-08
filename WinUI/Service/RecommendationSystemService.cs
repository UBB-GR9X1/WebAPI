using ClassLibrary.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUI.Model;
using WinUI.Repository;

namespace WinUI.Service
{
    internal class RecommendationSystemService : IRecommendationSystemService
    {
        private const int MaxNameLength = 100;
        private const int MaxEmailLength = 100;
        private const int MaxAvatarUrlLength = 255;
        private const int PhoneNumberLength = 10;

        private readonly IRecommendationSystemDoctorRepository _doctorDatabaseHelper;

        public RecommendationSystemDoctorModel DoctorInformation { get; private set; } = RecommendationSystemDoctorModel.Default;

        public List<RecommendationSystemDoctorModel> DoctorList { get; private set; }

        public RecommendationSystemService(IRecommendationSystemDoctorRepository doctorDbHelper)
        {
            _doctorDatabaseHelper = doctorDbHelper ?? throw new ArgumentNullException(nameof(doctorDbHelper));
            DoctorList = new List<RecommendationSystemDoctorModel>();
        }

        public async Task<List<RecommendationSystemDoctorJointModel>> GetDoctorsByDepartment(int departmentId) => 
            await _doctorDatabaseHelper.GetDoctorsByDepartment(departmentId);

        public async Task<List<RecommendationSystemDoctorJointModel>> GetAllDoctorsAsync() =>
            await _doctorDatabaseHelper.GetAllDoctors();

        public async Task<bool> LoadDoctorInformationByUserId(int doctorId)
        {
            try
            {
                DoctorInformation = await _doctorDatabaseHelper.GetDoctorById(doctorId);
                return DoctorInformation != RecommendationSystemDoctorModel.Default;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading doctor info: {ex.Message}", ex);
            }
        }

        public async Task<bool> SearchDoctorsByDepartmentAsync(string departmentPartialName)
        {
            DoctorList = await _doctorDatabaseHelper.GetDoctorsByDepartmentPartialName(departmentPartialName);
            return DoctorList != null;
        }

        public async Task<bool> SearchDoctorsByNameAsync(string namePartial)
        {
            DoctorList = await _doctorDatabaseHelper.GetDoctorsByPartialDoctorName(namePartial);
            return DoctorList != null;
        }

        public async Task<bool> UpdateDoctorName(int userId, string name)
        {
            ValidateDoctorName(name);
            return await _doctorDatabaseHelper.UpdateDoctorName(userId, name);
        }

        public async Task<bool> UpdateDepartment(int userId, int departmentId) =>
            await _doctorDatabaseHelper.UpdateDoctorDepartment(userId, departmentId);

        public async Task<bool> UpdateRatingAsync(int userId, double rating)
        {
            if (rating < 0.0 || rating > 5.0)
                throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 0 and 5.");

            return await _doctorDatabaseHelper.UpdateDoctorRating(userId, rating);
        }

        public async Task<bool> UpdateCareerInfo(int userId, string careerInfo)
        {
            careerInfo ??= string.Empty;
            return await _doctorDatabaseHelper.UpdateDoctorCareerInfo(userId, careerInfo);
        }

        public async Task<bool> UpdateAvatarUrl(int userId, string avatarUrl)
        {
            if (!string.IsNullOrEmpty(avatarUrl) && avatarUrl.Length > MaxAvatarUrlLength)
                throw new ArgumentException("Avatar URL is too long.", nameof(avatarUrl));

            return await _doctorDatabaseHelper.UpdateDoctorAvatarUrl(userId, avatarUrl ?? string.Empty);
        }

        public async Task<bool> UpdatePhoneNumber(int userId, string phoneNumber)
        {
            ValidatePhoneNumber(phoneNumber);
            return await _doctorDatabaseHelper.UpdateDoctorPhoneNumber(userId, phoneNumber ?? string.Empty);
        }

        public async Task<bool> UpdateEmail(int userId, string email)
        {
            ValidateEmail(email);
            return await _doctorDatabaseHelper.UpdateDoctorEmail(userId, email);
        }

        public async Task<bool> LogUpdate(int userId, ActionType action) =>
            await _doctorDatabaseHelper.UpdateLogService(userId, action);



        #region Validation Helpers

        private void ValidateDoctorName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || !name.Contains(' '))
                throw new ArgumentException("Doctor name must include at least a first and last name.", nameof(name));

            if (name.Length > MaxNameLength)
                throw new ArgumentException("Doctor name is too long.", nameof(name));
        }

        private void ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length != PhoneNumberLength)
                throw new ArgumentException($"Phone number must be exactly {PhoneNumberLength} digits.", nameof(phoneNumber));

            foreach (char c in phoneNumber)
            {
                if (!char.IsDigit(c))
                    throw new ArgumentException("Phone number must contain only digits.", nameof(phoneNumber));
            }
        }

        private void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Mail cannot be empty.", nameof(email));

            if (email.Length > MaxEmailLength)
                throw new ArgumentException("Mail is too long.", nameof(email));

            if (!email.Contains('@') || !email.Contains('.'))
                throw new ArgumentException("Mail must contain '@' and '.'.", nameof(email));
        }

        #endregion
    }
}
