using Microsoft.AspNetCore.Mvc;
using Data;
using Entity;
using ClassLibrary.IRepository;
using ClassLibrary.Domain;

namespace Controllers
{
    [Route("api/doctor")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorRepository doctorRepository;

        public DoctorController(IDoctorRepository _doctorRepository)
        {
            this.doctorRepository = _doctorRepository;
        }

        /// <summary>
        /// Retrieves all doctors.
        /// </summary>
        /// <returns>An ActionResult containing a list of doctors.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Doctor>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Doctor>>> GetAllDoctors()
        {
            try
            {
                List<Doctor> doctors = await this.doctorRepository.getAllDoctorsAsync();
                return this.Ok(doctors);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving doctors. Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a doctor by user ID.
        /// </summary>
        /// <param name="id">The ID of the doctor to retrieve.</param>
        /// <returns>An ActionResult containing the doctor.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Doctor), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Doctor>> GetDoctorByUserId(int id)
        {
            try
            {
                Doctor doctor = await this.doctorRepository.getDoctorByUserIdAsync(id);
                if (doctor == null)
                {
                    return this.NotFound($"Doctor with user ID {id} was not found.");
                }
                return this.Ok(doctor);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving the doctor. Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a list of doctors by department ID.
        /// </summary>
        /// <param name="departmentId">The department ID to retrieve doctors for.</param>
        /// <returns>An ActionResult containing the list of doctors in that department.</returns>
        [HttpGet("doctor/{departmentId}")]
        [ProducesResponseType(typeof(List<Doctor>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Doctor>>> GetDoctorsByDepartmentId(int departmentId)
        {
            try
            {
                List<Doctor> doctors = await this.doctorRepository.getDoctorsByDepartmentIdAsync(departmentId);
                return this.Ok(doctors);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving doctors. Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new doctor.
        /// </summary>
        /// <param name="doctor">The doctor entity to create.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateDoctor([FromBody] Doctor doctor)
        {
            if (doctor == null)
            {
                return this.BadRequest("Valid doctor entity is required.");
            }

            try
            {
                await this.doctorRepository.addDoctorAsync(doctor);
                return this.CreatedAtAction(nameof(GetDoctorByUserId), new { id = doctor.userId }, doctor);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while creating doctor. Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a doctor by user ID.
        /// </summary>
        /// <param name="id">The ID of the doctor to delete.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteDoctor(int id)
        {
            try
            {
                await this.doctorRepository.deleteDoctorAsync(id);
                return this.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound($"Doctor with user ID {id} was not found.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting doctor. Error: {ex.Message}");
            }
        }
    }
}
