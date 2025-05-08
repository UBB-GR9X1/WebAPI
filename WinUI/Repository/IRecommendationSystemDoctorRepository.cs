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
        Task<List<RecommendationSystemDoctorModel>> GetDoctorsByDepartmentPartialName(string departmentPartialName);

        Task<List<RecommendationSystemDoctorModel>> GetDoctorsByPartialDoctorName(string doctorPartialName);

        Task<List<RecommendationSystemDoctorJointModel>> GetDoctorsByDepartment(int departmentId);

        Task<List<RecommendationSystemDoctorJointModel>> GetAllDoctors();

        Task<RecommendationSystemDoctorModel> GetDoctorById(int doctorId);

        Task<bool> UpdateDoctorName(int userId, string name);

        Task<bool> UpdateDoctorEmail(int userId, string email);

        Task<bool> UpdateDoctorCareerInfo(int userId, string careerInfo);

        Task<bool> UpdateDoctorDepartment(int userId, int departmentId);

        Task<bool> UpdateDoctorRating(int userId, double rating);

        Task<bool> UpdateDoctorAvatarUrl(int userId, string newAvatarUrl);

        Task<bool> UpdateDoctorPhoneNumber(int userId, string newPhoneNumber);

        Task<bool> UpdateLogService(int userId, ActionType type);
    }
}
