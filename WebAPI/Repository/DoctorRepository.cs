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
        public async Task<List<Doctor>> GetAllDoctorsAsync()
        {
            List<DoctorEntity> doctorEntities = await dbContext.Doctors
                .Include(d => d.Department)
                .Include(d => d.User)
                .ToListAsync();

            List<Doctor> doctors = doctorEntities.Select(d => new Doctor
            {
                UserId = d.UserId,
                DepartmentId = d.DepartmentId,
                DoctorRating = d.DoctorRating,
                LicenseNumber = d.LicenseNumber,
                Name = d.User.Name
            }).ToList();

            return doctors;
        }

        /// <inheritdoc/>
        public async Task<Doctor> GetDoctorByUserIdAsync(int id)
        {
            var doctorEntity = await dbContext.Doctors
                .Include(d => d.Department)
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.UserId == id);

            if (doctorEntity == null)
            {
                throw new Exception($"Doctor with user ID {id} not found.");
            }

            return new Doctor
            {
                UserId = doctorEntity.UserId,
                DepartmentId = doctorEntity.DepartmentId,
                DoctorRating = doctorEntity.DoctorRating,
                LicenseNumber = doctorEntity.LicenseNumber,
                Name = doctorEntity.User.Name
            };
        }

        /// <inheritdoc/>
        public async Task<List<Doctor>> GetDoctorsByDepartmentIdAsync(int departmentId)
        {
            var doctorEntities = await dbContext.Doctors
                .Include(d => d.Department)
                .Include(d => d.User)
                .Where(d => d.DepartmentId == departmentId)
                .ToListAsync();

            return doctorEntities.Select(d => new Doctor
            {
                UserId = d.UserId,
                DepartmentId = d.DepartmentId,
                DoctorRating = d.DoctorRating,
                LicenseNumber = d.LicenseNumber,
                Name = d.User.Name
            }).ToList();
        }

        /// <inheritdoc/>
        public async Task AddDoctorAsync(Doctor doctor)
        {
            var existingPatient = await dbContext.Patients.FindAsync(doctor.UserId);
            if (existingPatient != null)
            {
                throw new InvalidOperationException("User is already registered as a patient and cannot be a doctor.");
            }

            var doctorEntity = new DoctorEntity
            {
                UserId = doctor.UserId,
                DepartmentId = doctor.DepartmentId,
                DoctorRating = doctor.DoctorRating,
                LicenseNumber = doctor.LicenseNumber
            };

            dbContext.Doctors.Add(doctorEntity);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteDoctorAsync(int id)
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