using Microsoft.AspNetCore.Mvc;
using Data;
using Entity;
using ClassLibrary.Repository;
using ClassLibrary.Domain;

namespace Controllers
{
    [Route("api/log")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogRepository _log_repository;

        public LogController(ILogRepository _log_repository)
        {
            this._log_repository = _log_repository;
        }

        /// <summary>
        /// Retrieves all logs.
        /// </summary>
        /// <returns>An ActionResult containing a list of logs.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Log>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Log>>> getAllLogs()
        {
            try
            {
                List<Log> logs = await this._log_repository.getAllLogsAsync();
                return this.Ok(logs);
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving logs. Error: {exception.Message}");
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
        public async Task<ActionResult<Log>> getLogById(int id)
        {
            try
            {
                Log log = await this._log_repository.getLogByIdAsync(id);
                if (log == null)
                {
                    return this.NotFound($"Log with ID {id} was not found.");
                }
                return this.Ok(log);
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving the log. Error: {exception.Message}");
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
        public async Task<ActionResult> createLog([FromBody] Log log)
        {
            if (log == null)
            {
                return this.BadRequest("Valid log entity is required.");
            }

            try
            {
                await this._log_repository.addLogAsync(log);
                return this.CreatedAtAction(nameof(getLogById), new { id = log.logId }, log);
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while creating log. Error: {exception.Message}");
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
        public async Task<ActionResult> deleteLog(int id)
        {
            try
            {
                await this._log_repository.deleteLogAsync(id);
                return this.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound($"Log with ID {id} was not found.");
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting log. Error: {exception.Message}");
            }
        }
    }
}
