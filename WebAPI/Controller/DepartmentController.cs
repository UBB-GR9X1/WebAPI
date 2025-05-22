using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Entity;
using ClassLibrary.Repository;
using ClassLibrary.Domain;
using System;

namespace Controllers
{
    [Route("api/department")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _department_repository;

        public DepartmentController(IDepartmentRepository department_repository)
        {
            this._department_repository = department_repository;
        }

        /// <summary>
        /// Retrieves all departments.
        /// </summary>
        /// <returns>An ActionResult containing a list of departments.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Department>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Department>>> getAllDepartments()
        {
            try
            {
                List<Department> departments = await this._department_repository.getAllDepartmentsAsync();
                return this.Ok(departments);
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving departments. Error: {exception.Message}");
            }
        }

        /// <summary>
        /// Creates a new department.
        /// </summary>
        /// <param name="department">The department entity to create.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> createDepartment([FromBody] Department department)
        {
            if (department == null)
            {
                return this.BadRequest("Valid department entity is required.");
            }

            try
            {
                await this._department_repository.addDepartmentAsync(department);
                return this.CreatedAtAction(nameof(getAllDepartments), new { id = department.departmentId }, department);
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while creating department. Error: {exception.Message}");
            }
        }

        /// <summary>
        /// Deletes a department by its ID.
        /// </summary>
        /// <param name="id">The ID of the department to delete.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> deleteDepartment(int id)
        {
            try
            {
                await this._department_repository.deleteDepartmentAsync(id);
                return this.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound($"Department with ID {id} was not found.");
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting department. Error: {exception.Message}");
            }
        }

    }
}
