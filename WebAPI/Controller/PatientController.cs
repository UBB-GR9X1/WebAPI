using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Entity;
using ClassLibrary.IRepository;
using ClassLibrary.Domain;
using System;

namespace Controllers
{
    [Route("api/patient")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository _patient_repository;

        public PatientController(IPatientRepository patient_repository)
        {
            this._patient_repository = patient_repository;
        }

        /// <summary>
        /// Retrieves all patients.
        /// </summary>
        /// <returns>An ActionResult containing a list of patients.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Patient>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Patient>>> getAllPatients()
        {
            try
            {
                List<Patient> patients = await this._patient_repository.getAllPatientsAsync();
                return this.Ok(patients);
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving patients. Error: {exception.Message}");
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
        public async Task<ActionResult<Patient>> getPatientById(int id)
        {
            try
            {
                Patient patient = await this._patient_repository.getPatientByUserIdAsync(id);
                if (patient == null)
                {
                    return this.NotFound($"Patient with ID {id} was not found.");
                }
                return this.Ok(patient);
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving the patient. Error: {exception.Message}");
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
        public async Task<ActionResult> createPatient([FromBody] Patient patient)
        {
            if (patient == null)
            {
                return this.BadRequest("Valid patient entity is required.");
            }

            try
            {
                await this._patient_repository.addPatientAsync(patient);
                return this.CreatedAtAction(nameof(getPatientById), new { id = patient.userId }, patient);
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while creating patient. Error: {exception.Message}");
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
        public async Task<ActionResult> deletePatient(int id)
        {
            try
            {
                await this._patient_repository.deletePatientAsync(id);
                return this.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound($"Patient with ID {id} was not found.");
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting patient. Error: {exception.Message}");
            }
        }
    }
}
