using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUI.Model;

namespace WinUI.Repository
{
    internal interface IRecommendationSystemDoctorRepository
    {
        Task<List<RecommendationSystemDoctorModel>> getDoctorsByDepartmentPartialName(string department_partial_name);

        Task<List<RecommendationSystemDoctorModel>> getDoctorsByPartialDoctorName(string doctor_partial_name);

        Task<List<RecommendationSystemDoctorJointModel>> getDoctorsByDepartment(int department_id);

        Task<List<RecommendationSystemDoctorJointModel>> getAllDoctors();

        Task<RecommendationSystemDoctorModel> getDoctorById(int doctor_id);

        Task<bool> updateDoctorName(int user_id, string name);

        Task<bool> updateDoctorEmail(int user_id, string email);

        Task<bool> updateDoctorCareerInfo(int user_id, string career_info);

        Task<bool> updateDoctorDepartment(int user_id, int department_id);

        Task<bool> updateDoctorRating(int user_id, double rating);

        Task<bool> updateDoctorAvatarUrl(int user_id, string new_avatar_url);

        Task<bool> updateDoctorPhoneNumber(int user_id, string new_phone_number);

        Task<bool> updateLogService(int user_id, ActionType type);
    }
}
