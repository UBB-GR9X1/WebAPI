using System;
using System.Collections.Generic;
using ClassLibrary;
using System.Threading.Tasks;
using WinUI.Model;
using ClassLibrary.Domain;

namespace WinUI.Service
{
    public interface IDoctorService
    {
        Task<bool> LoadDoctorInformationByUserId(int userId);
        Task<bool> UpdateDoctorProfile(int userId, DoctorService.UpdateField field, string newValue, int? departmentId = null);
        Task<List<Department>> GetAllDepartments();
        DoctorModel DoctorInformation { get; }

    }
}
