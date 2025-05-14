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
        private const int _max_name_length = 100;
        private const int _max_email_length = 100;
        private const int _max_avatar_url_length = 255;
        private const int _phone_number_length = 10;

        private readonly IRecommendationSystemDoctorRepository _doctor_database_helper;

        public RecommendationSystemDoctorModel doctor_information { get; private set; } = RecommendationSystemDoctorModel.Default;

        public List<RecommendationSystemDoctorModel> doctor_list { get; private set; }

        public RecommendationSystemService(IRecommendationSystemDoctorRepository doctor_helper)
        {
            _doctor_database_helper = doctor_helper ?? throw new ArgumentNullException(nameof(doctor_helper));
            doctor_list = new List<RecommendationSystemDoctorModel>();
        }

        public async Task<List<RecommendationSystemDoctorJointModel>> getDoctorsByDepartment(int department_id) => 
            await _doctor_database_helper.getDoctorsByDepartment(department_id);

        public async Task<List<RecommendationSystemDoctorJointModel>> GetAllDoctorsAsync() =>
            await _doctor_database_helper.getAllDoctors();

        public async Task<bool> loadDoctorInformationByUserId(int doctor_id)
        {
            try
            {
                doctor_information = await _doctor_database_helper.getDoctorById(doctor_id);
                return doctor_information != RecommendationSystemDoctorModel.Default;
            }
            catch (Exception exception)
            {
                throw new Exception($"Error loading doctor info: {exception.Message}", exception);
            }
        }

        public async Task<bool> searchDoctorsByDepartmentAsync(string department_partial_name)
        {
            doctor_list = await _doctor_database_helper.getDoctorsByDepartmentPartialName(department_partial_name);
            return doctor_list != null;
        }

        public async Task<bool> searchDoctorsByNameAsync(string name_partial)
        {
            doctor_list = await _doctor_database_helper.getDoctorsByPartialDoctorName(name_partial);
            return doctor_list != null;
        }

        public async Task<bool> updateDoctorName(int user_id, string name)
        {
            validateDoctorName(name);
            return await _doctor_database_helper.updateDoctorName(user_id, name);
        }

        public async Task<bool> updateDepartment(int user_id, int department_id) =>
            await _doctor_database_helper.updateDoctorDepartment(user_id, department_id);

        public async Task<bool> updateRatingAsync(int user_id, double rating)
        {
            if (rating < 0.0 || rating > 5.0)
                throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 0 and 5.");

            return await _doctor_database_helper.updateDoctorRating(user_id, rating);
        }

        public async Task<bool> updateCareerInfo(int user_id, string career_info)
        {
            career_info ??= string.Empty;
            return await _doctor_database_helper.updateDoctorCareerInfo(user_id, career_info);
        }

        public async Task<bool> updateAvatarUrl(int user_id, string avatar_url)
        {
            if (!string.IsNullOrEmpty(avatar_url) && avatar_url.Length > _max_avatar_url_length)
                throw new ArgumentException("Avatar URL is too long.", nameof(avatar_url));

            return await _doctor_database_helper.updateDoctorAvatarUrl(user_id, avatar_url ?? string.Empty);
        }

        public async Task<bool> updatePhoneNumber(int user_id, string phone_number)
        {
            validatePhoneNumber(phone_number);
            return await _doctor_database_helper.updateDoctorPhoneNumber(user_id, phone_number ?? string.Empty);
        }

        public async Task<bool> updateEmail(int user_id, string email)
        {
            validateEmail(email);
            return await _doctor_database_helper.updateDoctorEmail(user_id, email);
        }

        public async Task<bool> logUpdate(int user_id, ActionType action) =>
            await _doctor_database_helper.updateLogService(user_id, action);



        #region Validation Helpers

        private void validateDoctorName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || !name.Contains(' '))
                throw new ArgumentException("Doctor name must include at least a first and last name.", nameof(name));

            if (name.Length > _max_name_length)
                throw new ArgumentException("Doctor name is too long.", nameof(name));
        }

        private void validatePhoneNumber(string phone_number)
        {
            if (string.IsNullOrWhiteSpace(phone_number) || phone_number.Length != _phone_number_length)
                throw new ArgumentException($"Phone number must be exactly {_phone_number_length} digits.", nameof(phone_number));

            foreach (char digit in phone_number)
            {
                if (!char.IsDigit(digit))
                    throw new ArgumentException("Phone number must contain only digits.", nameof(phone_number));
            }
        }

        private void validateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Mail cannot be empty.", nameof(email));

            if (email.Length > _max_email_length)
                throw new ArgumentException("Mail is too long.", nameof(email));

            if (!email.Contains('@') || !email.Contains('.'))
                throw new ArgumentException("Mail must contain '@' and '.'.", nameof(email));
        }

        #endregion
    }
}
