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
        private readonly ApplicationDbContext dbContext;

        public PatientRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <inheritdoc/>
        public async Task<List<Patient>> getAllPatientsAsync()
        {
            var patientEntities = await dbContext.Patients.ToListAsync();

            return patientEntities.Select(p => new Patient
            {
                userId = p.UserId,
                bloodType = p.BloodType,
                EmergencyContact = p.EmergencyContact,
                allergies = p.Allergies,
                weight = p.Weight,
                height = p.Height
            }).ToList();
        }

        /// <inheritdoc/>
        public async Task<Patient> getPatientByUserIdAsync(int id)
        {
            var patientEntity = await dbContext.Patients.FindAsync(id);

            if (patientEntity == null)
            {
                throw new Exception($"Patient with User ID {id} not found.");
            }

            return new Patient
            {
                userId = patientEntity.UserId,
                bloodType = patientEntity.BloodType,
                EmergencyContact = patientEntity.EmergencyContact,
                allergies = patientEntity.Allergies,
                weight = patientEntity.Weight,
                height = patientEntity.Height
            };
        }

        /// <inheritdoc/>
        public async Task addPatientAsync(Patient patient)
        {
            var existingDoctor = await dbContext.Doctors.FindAsync(patient.userId);
            if (existingDoctor != null)
            {
                throw new InvalidOperationException("User is already registered as a doctor and cannot be a patient.");
            }

            var patientEntity = new PatientEntity
            {
                UserId = patient.userId,
                BloodType = patient.bloodType,
                EmergencyContact = patient.EmergencyContact,
                Allergies = patient.allergies,
                Weight = patient.weight,
                Height = patient.height
            };

            dbContext.Patients.Add(patientEntity);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task deletePatientAsync(int id)
        {
            var patientEntity = await dbContext.Patients.FindAsync(id);
            if (patientEntity == null)
            {
                throw new Exception($"Patient with User ID {id} not found.");
            }

            dbContext.Patients.Remove(patientEntity);
            await dbContext.SaveChangesAsync();
        }
    }
}
