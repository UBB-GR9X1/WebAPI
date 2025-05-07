using System;
using System.Collections.Generic;
using ClassLibrary;
using System.Threading.Tasks;
using WinUI.Model;

namespace WinUI.Service
{
    public interface IDoctorService
    {
        Task<bool> LoadDoctorInformationByUserId(int userId);

        Task<bool> UpdateDoctorName(int userId, string newName);

        Task<bool> UpdateDepartment(int userId, int departmentId);

        Task<bool> UpdateCareerInfo(int userId, string careerInfo);

        Task<bool> UpdateAvatarUrl(int userId, string avatarUrl);

        Task<bool> UpdatePhoneNumber(int userId, string phoneNumber);

        Task<bool> UpdateEmail(int userId, string email);

        // Task<bool> LogUpdate(int userId, ActionType action);

        DoctorModel DoctorInformation { get; }

    }
}
