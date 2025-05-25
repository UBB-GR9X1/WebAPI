using Microsoft.AspNetCore.Mvc;
using Data;
using Entity;
using ClassLibrary.Repository;
using ClassLibrary.Domain;

namespace Controllers
{
    [Route("api/doctor")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorRepository _doctor_repository;

        public DoctorController(IDoctorRepository doctor_repository)
        {
            this._doctor_repository = doctor_repository;
        }

        /// <summary>
        /// Retrieves all doctors.
        /// </summary>
        /// <returns>An ActionResult containing a list of doctors.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Doctor>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Doctor>>> getAllDoctors()
        {
            try
            {
                List<Doctor> doctors = await this._doctor_repository.getAllDoctorsAsync();
                return this.Ok(doctors);
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving doctors. Error: {exception.Message}");
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
        public async Task<ActionResult<Doctor>> getDoctorByUserId(int id)
        {
            try
            {
                Doctor doctor = await this._doctor_repository.getDoctorByUserIdAsync(id);
                if (doctor == null)
                {
                    return this.NotFound($"Doctor with user ID {id} was not found.");
                }
                return this.Ok(doctor);
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving the doctor. Error: {exception.Message}");
            }
        }

        /// <summary>
        /// Retrieves a list of doctors by department ID.
        /// </summary>
        /// <param name="department_id">The department ID to retrieve doctors for.</param>
        /// <returns>An ActionResult containing the list of doctors in that department.</returns>
        [HttpGet("doctor/{departmentId}")]
        [ProducesResponseType(typeof(List<Doctor>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Doctor>>> getDoctorsByDepartmentId(int department_id)
        {
            try
            {
                List<Doctor> doctors = await this._doctor_repository.getDoctorsByDepartmentIdAsync(department_id);
                return this.Ok(doctors);
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving doctors. Error: {exception.Message}");
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
        public async Task<ActionResult> createDoctor([FromBody] Doctor doctor)
        {
            if (doctor == null)
            {
                return this.BadRequest("Valid doctor entity is required.");
            }

            try
            {
                await this._doctor_repository.addDoctorAsync(doctor);
                return this.CreatedAtAction(nameof(getDoctorByUserId), new { id = doctor.userId }, doctor);
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while creating doctor. Error: {exception.Message}");
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
        public async Task<ActionResult> deleteDoctor(int id)
        {
            try
            {
                await this._doctor_repository.deleteDoctorAsync(id);
                return this.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound($"Doctor with user ID {id} was not found.");
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting doctor. Error: {exception.Message}");
            }
        }
    }
}
