namespace WebApi.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using ClassLibrary.Repository;
    using Data;
    using ClassLibrary.Domain;
    using Entity;
    using global::Data;

    /// <summary>
    /// Repository class for managing department-related database operations.
    /// </summary>
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _db_context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepartmentRepository"/> class.
        /// </summary>
        /// <param name="db_context">The database context instance.</param>
        public DepartmentRepository(ApplicationDbContext db_context)
        {
            this._db_context = db_context;
        }

        /// <inheritdoc/>
        public async Task<List<Department>> getAllDepartmentsAsync()
        {
            // Retrieve all departments from the database.
            List<DepartmentEntity> department_entities = await this._db_context.Departments.ToListAsync();

            // Convert the list of DepartmentEntity to a list of Department models.
            List<Department> departments = department_entities
                .Select(departmentEntity => new Department
                {
                    departmentId = departmentEntity.id,
                    departmentName = departmentEntity.name
                })
                .ToList();

            return departments;
        }

        /// <inheritdoc/>
        public async Task<Department> getDepartmentByIdAsync(int id)
        {
            // Retrieve a department by its ID from the database.
            DepartmentEntity department_entity = await this._db_context.Departments.FindAsync(id);

            if (department_entity == null)
            {
                throw new Exception($"Department with ID {id} not found.");
            }

            // Return a new Department object (mapping from Entity to Domain model).
            return new Department
            {
                departmentId = department_entity.id,
                departmentName = department_entity.name
            };
        }

        /// <inheritdoc/>
        public async Task addDepartmentAsync(Department department)
        {
            // Map the Department model to the DepartmentEntity.
            var department_entity = new DepartmentEntity
            {
                name = department.departmentName
            };

            // Add the department to the database.
            this._db_context.Departments.Add(department_entity);
            await this._db_context.SaveChangesAsync();

            // Set the department's Id after it's saved.
            department.departmentId = department_entity.id;
        }

        /// <inheritdoc/>
        public async Task deleteDepartmentAsync(int id)
        {
            // Find the department by its ID.
            DepartmentEntity department_entity = await this._db_context.Departments.FindAsync(id);

            if (department_entity == null)
            {
                throw new Exception($"Department with ID {id} not found.");
            }

            // Remove the department from the database.
            this._db_context.Departments.Remove(department_entity);
            await this._db_context.SaveChangesAsync();
        }
    }
}
