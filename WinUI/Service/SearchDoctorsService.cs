using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.IRepository;
using ClassLibrary.Domain;
using WinUI.Model;
using WinUI.Proxy;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;

namespace WinUI.Service
{
    public class SearchDoctorsService : ISearchDoctorsService
    {
        private readonly DoctorsProxy _doctorsRepository;
        private readonly LogInProxy _userRepository;
        public List<DoctorModel> AvailableDoctors { get; private set; }

        public SearchDoctorsService(IDoctorRepository doctorsRepository)
        {
            _doctorsRepository = (DoctorsProxy?)doctorsRepository;
            AvailableDoctors = new List<DoctorModel>();
        }

        public async Task LoadDoctors(string searchTerm)
        {
            try
            {
                AvailableDoctors.Clear();
                var doctorsByDepartment = GetDoctorsByDepartmentPartialName(searchTerm);
                var doctorsByName = GetDoctorsByPartialDoctorName(searchTerm);

                foreach (var doctor in doctorsByDepartment)
                {
                    AvailableDoctors.Add(doctor);
                }

                foreach (var doctor in doctorsByName)
                {
                    var isAlreadyAdded = AvailableDoctors.Any(existingDoctor => existingDoctor.DoctorId == doctor.DoctorId);
                    if (!isAlreadyAdded)
                    {
                        AvailableDoctors.Add(doctor);
                    }
                }

                AvailableDoctors = SortDoctorsByDefaultCriteria(AvailableDoctors);
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error loading doctors: {error.Message}");
            }
        }

        public List<DoctorModel> GetDoctorsByDepartmentPartialName(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return new List<DoctorModel>();
                }

                var allDoctors = _doctorsRepository.GetAllDoctorsAsync().Result;
                if (allDoctors == null)
                {
                    return new List<DoctorModel>();
                }

                var filteredDoctors = allDoctors
                    .Where(doctor =>
                    {
                        var department = _doctorsRepository.GetDepartmentByIdAsync(doctor.DepartmentId).Result;
                        return department?.Name?.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0;
                    })
                    .Select(doctor => new DoctorModel
                    {
                        DoctorId = doctor.UserId, 
                        DoctorName = GetUserName(doctor.UserId), 
                        DepartmentId = doctor.DepartmentId,
                        DepartmentName = GetDepartmentName(doctor.DepartmentId), 
                        Rating = doctor.DoctorRating,
                        Mail = GetUserEmail(doctor.UserId)
                    })
                    .ToList();

                return filteredDoctors;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching doctors by department: {ex.Message}");
                return new List<DoctorModel>();
            }
        }

        public List<DoctorModel> GetDoctorsByPartialDoctorName(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return new List<DoctorModel>();
                }

                var allDoctors = _doctorsRepository.GetAllDoctorsAsync().Result;
                if (allDoctors == null)
                {
                    return new List<DoctorModel>();
                }

                var filteredDoctors = allDoctors
                    .Where(doctor =>
                    {
                        var user = _userRepository.getUserById(doctor.UserId).Result;
                        return user?.username?.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0;
                    })
                    .Select(doctor => new DoctorModel
                    {
                        DoctorId = doctor.UserId, 
                        DoctorName = GetUserName(doctor.UserId), 
                        DepartmentId = doctor.DepartmentId,
                        DepartmentName = GetDepartmentName(doctor.DepartmentId), 
                        Rating = doctor.DoctorRating, 
                        Mail = GetUserEmail(doctor.UserId)
                    })
                    .ToList();

                return filteredDoctors;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching doctors by name: {ex.Message}");
                return new List<DoctorModel>();
            }
        }

        private string GetUserName(int userId)
        {
            var user = _userRepository.getUserById(userId).Result;
            return user?.username ?? DoctorModel.DefaultName;
        }

        private string GetDepartmentName(int departmentId)
        {
            var department = _doctorsRepository.GetDepartmentByIdAsync(departmentId).Result;
            return department?.Name ?? DoctorModel.DefaultDepartmentName;
        }


        private string GetUserEmail(int userId)
        {
            var user = _userRepository.getUserById(userId).Result;
            return user?.mail ?? DoctorModel.DefaultEmail;
        }

        public List<DoctorModel> GetSearchedDoctors()
        {
            return AvailableDoctors;
        }

        public List<DoctorModel> GetDoctorsSortedBy(SortCriteria sortCriteria)
        {
            switch (sortCriteria)
            {
                case SortCriteria.RatingHighToLow:
                    return AvailableDoctors.OrderByDescending(doctor => doctor.Rating).ToList();
                case SortCriteria.RatingLowToHigh:
                    return AvailableDoctors.OrderBy(doctor => doctor.Rating).ToList();
                case SortCriteria.NameAscending:
                    return AvailableDoctors.OrderBy(doctor => doctor.DoctorName).ToList();
                case SortCriteria.NameDescending:
                    return AvailableDoctors.OrderByDescending(doctor => doctor.DoctorName).ToList();
                case SortCriteria.DepartmentAscending:
                    return AvailableDoctors.OrderBy(doctor => doctor.DepartmentName).ToList();
                case SortCriteria.RatingThenNameThenDepartment:
                    return SortDoctorsByDefaultCriteria(AvailableDoctors);
                default:
                    return AvailableDoctors;
            }
        }

        private List<DoctorModel> SortDoctorsByDefaultCriteria(List<DoctorModel> doctors)
        {
            return doctors
                .OrderByDescending(doctor => doctor.Rating)
                .ThenBy(doctor => doctor.DoctorName)
                .ThenBy(doctor => doctor.DepartmentName)
                .ToList();
        }
    }

    public enum SortCriteria
    {
        RatingHighToLow,
        RatingLowToHigh,
        NameAscending,
        NameDescending,
        DepartmentAscending,
        RatingThenNameThenDepartment
    }
}

public class InverseBoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool boolValue)
        {
            return boolValue ? Visibility.Collapsed : Visibility.Visible;
        }

        return Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is Visibility visibility)
        {
            return visibility == Visibility.Collapsed;
        }

        return false;
    }
}

public class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return value is Visibility && (Visibility)value == Visibility.Visible;
    }
}