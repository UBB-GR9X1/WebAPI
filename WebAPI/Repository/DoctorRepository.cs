using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.Repository;
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
        private readonly ApplicationDbContext _db_context;

        public DoctorRepository(ApplicationDbContext db_context)
        {
            this._db_context = db_context;
        }

        /// <inheritdoc/>
        public async Task<List<Doctor>> getAllDoctorsAsync()
        {
            List<DoctorEntity> doctor_entities = await this._db_context.Doctors
                .Include(doctor => doctor.department)
                .Include(doctor => doctor.user)
                .ToListAsync();

            List<Doctor> doctors = doctor_entities.Select(doctor => new Doctor
            {
                userId = doctor.userId,
                departmentId = doctor.departmentId,
                doctorRating = doctor.doctorRating,
                licenseNumber = doctor.licenseNumber,
                doctorName = doctor.user.name
            }).ToList();

            return doctors;
        }

        /// <inheritdoc/>
        public async Task<Doctor> getDoctorByUserIdAsync(int id)
        {
            DoctorEntity doctor_entity = await this._db_context.Doctors
                .Include(doctor => doctor.department)
                .Include(doctor => doctor.user)
                .FirstOrDefaultAsync(doctor => doctor.userId == id);

            if (doctor_entity == null)
            {
                throw new Exception($"Doctor with user ID {id} not found.");
            }

            return new Doctor
            {
                userId = doctor_entity.userId,
                departmentId = doctor_entity.departmentId,
                doctorRating = doctor_entity.doctorRating,
                licenseNumber = doctor_entity.licenseNumber,
                doctorName = doctor_entity.user.name
            };
        }

        /// <inheritdoc/>
        public async Task<List<Doctor>> getDoctorsByDepartmentIdAsync(int department_id)
        {
            List<DoctorEntity> doctor_entities = await _db_context.Doctors
                .Include(doctor => doctor.department)
                .Include(doctor => doctor.user)
                .Where(doctor => doctor.departmentId == department_id)
                .ToListAsync();

            return doctor_entities.Select(doctor => new Doctor
            {
                userId = doctor.userId,
                departmentId = doctor.departmentId,
                doctorRating = doctor.doctorRating,
                licenseNumber = doctor.licenseNumber,
                doctorName = doctor.user.name
            }).ToList();
        }

        /// <inheritdoc/>
        public async Task addDoctorAsync(Doctor doctor)
        {
            var existing_patient = await _db_context.Patients.FindAsync(doctor.userId);
            if (existing_patient != null)
            {
                throw new InvalidOperationException("User is already registered as a patient and cannot be a doctor.");
            }

            var doctor_entity = new DoctorEntity
            {
                userId = doctor.userId,
                departmentId = doctor.departmentId,
                doctorRating = doctor.doctorRating,
                licenseNumber = doctor.licenseNumber
            };

            _db_context.Doctors.Add(doctor_entity);
            await _db_context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task deleteDoctorAsync(int id)
        {
            var doctor_entity = await _db_context.Doctors.FindAsync(id);
            if (doctor_entity == null)
            {
                throw new Exception($"Doctor with user ID {id} not found.");
            }

            _db_context.Doctors.Remove(doctor_entity);
            await _db_context.SaveChangesAsync();
        }
    }
}