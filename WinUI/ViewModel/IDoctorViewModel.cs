using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;

namespace WinUI.ViewModel
{
    public interface IDoctorViewModel
    {
        Visibility MainContentVisibility { get; }
        int UserId { get; set; }

        string DoctorName { get; set; }

        string DepartmentName { get; set; }

        double Rating { get; set; }

        string CareerInfo { get; set; }

        string AvatarUrl { get; set; }

        string PhoneNumber { get; set; }

        string Mail { get; set; }

        // Added for XAML binding
        bool IsLoading { get; set; }
        int DepartmentId { get; set; }

        Task<bool> LoadDoctorInformationAsync(int userId);

        Task<(bool updateSuccessful, string? errorMessage)> TryUpdateDoctorProfileAsync();

        void RevertChanges();
    }
}
