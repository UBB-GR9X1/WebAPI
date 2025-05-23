    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ClassLibrary.Domain;
    using ClassLibrary.Repository;
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
                    await _userRepository.addUserAsync(user);

                    var allUsers = await _userRepository.getAllUsersAsync();

                    var updatedUser = allUsers.FirstOrDefault(u =>
                        u.name == user.name &&
                        u.mail == user.mail &&
                        u.role == user.role);

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
                    var doctor = await _doctorRepository.getDoctorByUserIdAsync(userId);
                    if (doctor == null)
                        return false;

                    var user = await _userRepository.getUserByIdAsync(userId);
                    var department = await _doctorRepository.getDepartmentByIdAsync(doctor.departmentId);
                    if (user == null || department == null)
                        return false;

                    DoctorInformation = new DoctorModel
                    {
                        DoctorId = user.userId,
                        DoctorName = user.name,
                        DepartmentId = department.departmentId,
                        DepartmentName = department.departmentName,
                        Rating = doctor.doctorRating,
                        Mail = user.mail,
                        PhoneNumber = user.phoneNumber,
                        CareerInfo = user.role, // Placeholder if CareerInfo is not defined elsewhere
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
            Mail
        }

        public async Task<bool> UpdateDoctorProfile(int userId, UpdateField field, string newValue, int? departmentId = null)
        {
            try
            {
                var user = await _userRepository.getUserByIdAsync(userId);
                if (user == null) return false;

                var doctor = await _doctorRepository.getDoctorByUserIdAsync(userId);
                if (doctor == null) return false;

                // Update fields accordingly
                switch (field)
                {
                    case UpdateField.DoctorName:
                        user.name = newValue;
                        break;
                    case UpdateField.Department:
                        if (departmentId.HasValue)
                            doctor.departmentId = departmentId.Value;
                        break;
                    case UpdateField.CareerInfo:
                        user.role = newValue;
                        break;
                    case UpdateField.AvatarUrl:
                        user.address = newValue;
                        break;
                    case UpdateField.PhoneNumber:
                        user.phoneNumber = newValue;
                        break;
                    case UpdateField.Mail:
                        user.mail = newValue;
                        break;
                    default:
                        throw new ArgumentException("Invalid update field specified");
                }

                // Update User first
                await _userRepository.updateUserAsync(user);
                // Update the doctor if needed
                if (field == UpdateField.Department)
                {
                    await _doctorRepository.updateDoctorByIdAsync(doctor.userId, doctor);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating doctor profile: {ex.Message}");
                return false;
            }
        }
        public async Task<List<Department>> GetAllDepartments()
        {
            try
            {
                return await _doctorRepository.GetAllDepartmentsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting departments: {ex.Message}");
                return new List<Department>();
            }
        }

    }
}