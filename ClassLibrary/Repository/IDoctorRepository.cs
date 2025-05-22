using ClassLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Repository
{
   public interface IDoctorRepository
    {
        /// <summary>
        /// Gets all doctors.
        /// </summary>
        /// <returns>A list of doctors</returns>
        Task<List<Doctor>> getAllDoctorsAsync();

        /// <summary>
        /// Gets a doctor by its unique identifier.
        /// </summary>
        /// <param name="id">The id of the user</param>
        /// <returns>The doctor with the given user id.</returns>
        Task<Doctor> getDoctorByUserIdAsync(int id);

        /// <summary>
        /// Gets a list of doctors by department id.
        /// </summary>
        /// <param name="department_id">The id of the departemnt</param>
        /// <returns>The list of doctors with the given department id</returns>
        Task<List<Doctor>> getDoctorsByDepartmentIdAsync(int department_id);

        Task addDoctorAsync(Doctor doctor);

        Task deleteDoctorAsync(int id);
    }
}
