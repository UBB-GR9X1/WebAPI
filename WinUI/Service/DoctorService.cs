    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ClassLibrary.Domain;
    using ClassLibrary.IRepository;
    using WinUI.Model;
    using WinUI.Proxy;
    using WinUI.Model;
    using WinUI.Repository;
    using System.Linq;

    namespace WinUI.Service
    {
        public class DoctorService : IDoctorService
        {
            private readonly DoctorsProxy _doctorRepository;
            private readonly UserProxy _userRepository;

            public DoctorModel DoctorInformation { get; private set; } = DoctorModel.Default;

            public DoctorService(IDoctorRepository doctorRepository, IUserRepository userRepository)
            {
                _doctorRepository = (DoctorsProxy?)(doctorRepository ?? throw new ArgumentNullException(nameof(doctorRepository)));
                _userRepository = (UserProxy?)userRepository;
            }

            public async Task<User> AddUserAndGetUpdatedIdAsync(User user)
            {
                try
                {
                    await _userRepository.AddUserAsync(user);

                    var allUsers = await _userRepository.GetAllUsersAsync();

                    var updatedUser = allUsers.FirstOrDefault(u =>
                        u.Name == user.Name &&
                        u.Mail == user.Mail &&
                        u.Role == user.Role);

                    if (updatedUser == null)
                    {
                        throw new Exception("Could not find the newly added user in the database.");
                    }

                    return updatedUser;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding user and getting updated ID: {ex.Message}");
                    throw;
                }
            }



            public async Task<bool> LoadDoctorInformationByUserId(int userId)
            {
                try
                {
                    var doctor = await _doctorRepository.GetDoctorByUserIdAsync(userId);
                    if (doctor == null)
                        return false;

                    var user = await _userRepository.GetUserByIdAsync(userId);
                    var department = await _doctorRepository.GetDepartmentByIdAsync(doctor.DepartmentId);
                    if (user == null || department == null)
                        return false;

                    DoctorInformation = new DoctorModel
                    {
                        DoctorId = user.UserId,
                        DoctorName = user.Name,
                        DepartmentId = department.Id,
                        DepartmentName = department.Name,
                        Rating = doctor.DoctorRating,
                        Mail = user.Mail,
                        CareerInfo = user.Role, // Placeholder if CareerInfo is not defined elsewhere
                        AvatarUrl = "" // Placeholder; update if this exists in your model
                    };

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading doctor info: {ex.Message}");
                    return false;
                }
            }

        public enum UpdateField
        {
            DoctorName,
            Department,
            CareerInfo,
            AvatarUrl,
            PhoneNumber,
            Email
        }

        public async Task<bool> UpdateDoctorProfile(int userId, UpdateField field, string newValue, int? departmentId = null)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null) return false;

                var doctor = await _doctorRepository.GetDoctorByUserIdAsync(userId);
                if (doctor == null) return false;

                bool isUserUpdate = false;

                switch (field)
                {
                    case UpdateField.DoctorName:
                        user.Name = newValue;
                        isUserUpdate = true;
                        break;
                    case UpdateField.Department:
                        if (departmentId.HasValue)
                            doctor.DepartmentId = departmentId.Value;
                        break;
                    case UpdateField.CareerInfo:
                        user.Role = newValue;
                        isUserUpdate = true;
                        break;
                    case UpdateField.AvatarUrl:
                        user.Address = newValue;
                        isUserUpdate = true;
                        break;
                    case UpdateField.PhoneNumber:
                        user.PhoneNumber = newValue;
                        isUserUpdate = true;
                        break;
                    case UpdateField.Email:
                        user.Mail = newValue;
                        isUserUpdate = true;
                        break;
                    default:
                        throw new ArgumentException("Invalid update field specified");
                }

                if (isUserUpdate)
                {
                    await _doctorRepository.DeleteDoctorAsync(doctor.UserId);
                    await _userRepository.DeleteUserAsync(userId);

                    var updatedUser = await AddUserAndGetUpdatedIdAsync(user);

                    await _doctorRepository.AddDoctorAsync(new Doctor
                    {
                        UserId = updatedUser.UserId,
                        DepartmentId = doctor.DepartmentId,
                        DoctorRating = doctor.DoctorRating,
                        LicenseNumber = doctor.LicenseNumber
                    });
                }
                else
                {
                    await _doctorRepository.DeleteDoctorAsync(doctor.UserId);
                    await _doctorRepository.AddDoctorAsync(doctor);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating doctor profile: {ex.Message}");
                return false;
            }
        }
    }
    }