using Microsoft.AspNetCore.Mvc;
using Data;
using Entity;
using ClassLibrary.IRepository;
using ClassLibrary.Domain;

namespace Controllers
{
    [Route("api/log")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogRepository logRepository;

        public LogController(ILogRepository _logRepository)
        {
            this.logRepository = _logRepository;
        }

        /// <summary>
        /// Retrieves all logs.
        /// </summary>
        /// <returns>An ActionResult containing a list of logs.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Log>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Log>>> GetAllLogs()
        {
            try
            {
                List<Log> logs = await this.logRepository.getAllLogsAsync();
                return this.Ok(logs);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving logs. Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a log by ID.
        /// </summary>
        /// <param name="id">The ID of the log to retrieve.</param>
        /// <returns>An ActionResult containing the log.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Log), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Log>> GetLogById(int id)
        {
            try
            {
                Log log = await this.logRepository.getLogByIdAsync(id);
                if (log == null)
                {
                    return this.NotFound($"Log with ID {id} was not found.");
                }
                return this.Ok(log);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving the log. Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new log.
        /// </summary>
        /// <param name="log">The log entity to create.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateLog([FromBody] Log log)
        {
            if (log == null)
            {
                return this.BadRequest("Valid log entity is required.");
            }

            try
            {
                await this.logRepository.addLogAsync(log);
                return this.CreatedAtAction(nameof(GetLogById), new { id = log.logId }, log);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while creating log. Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a log by its ID.
        /// </summary>
        /// <param name="id">The ID of the log to delete.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteLog(int id)
        {
            try
            {
                await this.logRepository.deleteLogAsync(id);
                return this.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound($"Log with ID {id} was not found.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting log. Error: {ex.Message}");
            }
        }
    }
}
