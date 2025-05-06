using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Entity;
using ClassLibrary.IRepository;
using ClassLibrary.Domain;

namespace Controllers
{
    [Route("api/patient")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository patientRepository;

        public PatientController(IPatientRepository _patientRepository)
        {
            this.patientRepository = _patientRepository;
        }

        /// <summary>
        /// Retrieves all patients.
        /// </summary>
        /// <returns>An ActionResult containing a list of patients.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Patient>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Patient>>> GetAllPatients()
        {
            try
            {
                List<Patient> patients = await this.patientRepository.getAllPatientsAsync();
                return this.Ok(patients);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving patients. Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a patient by ID.
        /// </summary>
        /// <param name="id">The ID of the patient to retrieve.</param>
        /// <returns>An ActionResult containing the patient.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Patient), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Patient>> GetPatientById(int id)
        {
            try
            {
                Patient patient = await this.patientRepository.getPatientByUserIdAsync(id);
                if (patient == null)
                {
                    return this.NotFound($"Patient with ID {id} was not found.");
                }
                return this.Ok(patient);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving the patient. Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new patient.
        /// </summary>
        /// <param name="patient">The patient entity to create.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreatePatient([FromBody] Patient patient)
        {
            if (patient == null)
            {
                return this.BadRequest("Valid patient entity is required.");
            }

            try
            {
                await this.patientRepository.addPatientAsync(patient);
                return this.CreatedAtAction(nameof(GetPatientById), new { id = patient.userId }, patient);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while creating patient. Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a patient by its ID.
        /// </summary>
        /// <param name="id">The ID of the patient to delete.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeletePatient(int id)
        {
            try
            {
                await this.patientRepository.deletePatientAsync(id);
                return this.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound($"Patient with ID {id} was not found.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting patient. Error: {ex.Message}");
            }
        }
    }
}
