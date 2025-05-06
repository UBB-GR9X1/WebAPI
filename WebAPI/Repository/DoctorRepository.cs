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
    /// Repository class for managing doctor-related database operations.
    /// </summary>
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ApplicationDbContext dbContext;

        public DoctorRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <inheritdoc/>
        public async Task<List<Doctor>> getAllDoctorsAsync()
        {
            List<DoctorEntity> doctorEntities = await dbContext.Doctors.Include(d => d.Department).ToListAsync();

            List<Doctor> doctors = doctorEntities.Select(d => new Doctor
            {
                userId = d.UserId,
                departmentId = d.DepartmentId,
                doctorRating = d.DoctorRating,
                licenseNumber = d.LicenseNumber
            }).ToList();

            return doctors;
        }

        /// <inheritdoc/>
        public async Task<Doctor> getDoctorByUserIdAsync(int id)
        {
            var doctorEntity = await dbContext.Doctors
                .Include(d => d.Department)
                .FirstOrDefaultAsync(d => d.UserId == id);

            if (doctorEntity == null)
            {
                throw new Exception($"Doctor with user ID {id} not found.");
            }

            return new Doctor
            {
                userId = doctorEntity.UserId,
                departmentId = doctorEntity.DepartmentId,
                doctorRating = doctorEntity.DoctorRating,
                licenseNumber = doctorEntity.LicenseNumber
            };
        }

        /// <inheritdoc/>
        public async Task<List<Doctor>> getDoctorsByDepartmentIdAsync(int departmentId)
        {
            var doctorEntities = await dbContext.Doctors
                .Where(d => d.DepartmentId == departmentId)
                .ToListAsync();

            return doctorEntities.Select(d => new Doctor
            {
                userId = d.UserId,
                departmentId = d.DepartmentId,
                doctorRating = d.DoctorRating,
                licenseNumber = d.LicenseNumber
            }).ToList();
        }

        /// <inheritdoc/>
        public async Task addDoctorAsync(Doctor doctor)
        {
            var existingPatient = await dbContext.Patients.FindAsync(doctor.userId);
            if (existingPatient != null)
            {
                throw new InvalidOperationException("User is already registered as a patient and cannot be a doctor.");
            }

            var doctorEntity = new DoctorEntity
            {
                UserId = doctor.userId,
                DepartmentId = doctor.departmentId,
                DoctorRating = doctor.doctorRating,
                LicenseNumber = doctor.licenseNumber
            };

            dbContext.Doctors.Add(doctorEntity);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task deleteDoctorAsync(int id)
        {
            var doctorEntity = await dbContext.Doctors.FindAsync(id);
            if (doctorEntity == null)
            {
                throw new Exception($"Doctor with user ID {id} not found.");
            }

            dbContext.Doctors.Remove(doctorEntity);
            await dbContext.SaveChangesAsync();
        }
    }
}