using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUI.Model;

namespace WinUI.Service
{
    internal interface IRecommendationSystemService
    {
        Task<bool> loadDoctorInformationByUserId(int user_id);

        Task<bool> updateDoctorName(int user_id, string new_name);

        Task<bool> updateDepartment(int user_id, int department_id);

        Task<bool> updateCareerInfo(int user_id, string career_info);

        Task<bool> updateAvatarUrl(int user_id, string avatar_url);

        Task<bool> updatePhoneNumber(int user_id, string phone_number);

        Task<bool> updateEmail(int user_id, string email);

        Task<bool> logUpdate(int user_id, ActionType action);

        RecommendationSystemDoctorModel doctor_information { get; }
    }
}
