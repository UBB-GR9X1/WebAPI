using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Entity;
using ClassLibrary.IRepository;
using ClassLibrary.Domain;

namespace Controllers
{
    [Route("api/department")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository departmentRepository;

        public DepartmentController(IDepartmentRepository _departmentRepository)
        {
            this.departmentRepository = _departmentRepository;
        }

        /// <summary>
        /// Retrieves all departments.
        /// </summary>
        /// <returns>An ActionResult containing a list of departments.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Department>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Department>>> GetAllDepartments()
        {
            try
            {
                List<Department> departments = await this.departmentRepository.getAllDepartmentsAsync();
                return this.Ok(departments);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving departments. Error: {ex.Message}");
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
        public async Task<ActionResult> CreateDepartment([FromBody] Department department)
        {
            if (department == null)
            {
                return this.BadRequest("Valid department entity is required.");
            }

            try
            {
                await this.departmentRepository.addDepartmentAsync(department);
                return this.CreatedAtAction(nameof(GetAllDepartments), new { id = department.id }, department);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while creating department. Error: {ex.Message}");
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
        public async Task<ActionResult> DeleteDepartment(int id)
        {
            try
            {
                await this.departmentRepository.deleteDepartmentAsync(id);
                return this.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound($"Department with ID {id} was not found.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting department. Error: {ex.Message}");
            }
        }

    }
}
