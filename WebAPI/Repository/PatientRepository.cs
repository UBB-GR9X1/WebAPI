using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.IRepository;
using Data;
using ClassLibrary.Domain;
using Entity;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Repository
{
    /// <summary>
    /// Repository class for managing patient-related database operations.
    /// </summary>
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _db_context;

        public PatientRepository(ApplicationDbContext db_context)
        {
            this._db_context = db_context;
        }

        /// <inheritdoc/>
        public async Task<List<Patient>> getAllPatientsAsync()
        {
            List<PatientEntity> patient_entities = await _db_context.Patients.ToListAsync();

            return patient_entities.Select(patient => new Patient
            {
                userId = patient.userId,
                bloodType = patient.bloodType,
                EmergencyContact = patient.emergencyContact,
                allergies = patient.allergies,
                weight = patient.weight,
                height = patient.height
            }).ToList();
        }

        /// <inheritdoc/>
        public async Task<Patient> getPatientByUserIdAsync(int id)
        {
            PatientEntity patient_entity = await _db_context.Patients.FindAsync(id);

            if (patient_entity == null)
            {
                throw new Exception($"Patient with User ID {id} not found.");
            }

            return new Patient
            {
                userId = patient_entity.userId,
                bloodType = patient_entity.bloodType,
                EmergencyContact = patient_entity.emergencyContact,
                allergies = patient_entity.allergies,
                weight = patient_entity.weight,
                height = patient_entity.height
            };
        }

        /// <inheritdoc/>
        public async Task addPatientAsync(Patient patient)
        {
            DoctorEntity existing_doctor = await _db_context.Doctors.FindAsync(patient.userId);
            if (existing_doctor != null)
            {
                throw new InvalidOperationException("User is already registered as a doctor and cannot be a patient.");
            }

            var patient_entity = new PatientEntity
            {
                userId = patient.userId,
                bloodType = patient.bloodType,
                emergencyContact = patient.EmergencyContact,
                allergies = patient.allergies,
                weight = patient.weight,
                height = patient.height
            };

            _db_context.Patients.Add(patient_entity);
            await _db_context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task deletePatientAsync(int id)
        {
            PatientEntity patient_entity = await _db_context.Patients.FindAsync(id);
            if (patient_entity == null)
            {
                throw new Exception($"Patient with User ID {id} not found.");
            }

            _db_context.Patients.Remove(patient_entity);
            await _db_context.SaveChangesAsync();
        }

        public Task updatePatientAsync(Patient patient, User user)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> getAllUserAsync()
        {
            throw new NotImplementedException();
        }
    }
}
