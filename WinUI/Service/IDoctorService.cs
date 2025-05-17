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
        Task<bool> UpdateDoctorProfile(int userId, DoctorService.UpdateField field, string newValue, int? departmentId = null);

        DoctorModel DoctorInformation { get; }

    }
}
