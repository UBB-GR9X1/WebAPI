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
    /// Repository class for managing log-related database operations.
    /// </summary>
    public class LogRepository : ILogRepository
    {
        private readonly ApplicationDbContext dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public LogRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <inheritdoc/>
        public async Task<List<Log>> getAllLogsAsync()
        {
            List<LogEntity> logEntities = await dbContext.Logs.ToListAsync();

            return logEntities.Select(log => new Log
            {
                logId = log.LogId,
                userId = (int)log.UserId,
                actionType = log.ActionType,
                timestamp = log.Timestamp
            }).ToList();
        }

        /// <inheritdoc/>
        public async Task<Log> getLogByIdAsync(int id)
        {
            var logEntity = await dbContext.Logs.FindAsync(id);
            if (logEntity == null)
            {
                throw new Exception($"Log with ID {id} not found.");
            }

            return new Log
            {
                logId = logEntity.LogId,
                userId = (int)logEntity.UserId,
                actionType = logEntity.ActionType,
                timestamp = logEntity.Timestamp
            };
        }

        /// <inheritdoc/>
        public async Task<Log> getLogByUserIdAsync(int userId)
        {
            var logEntity = await dbContext.Logs
                .FirstOrDefaultAsync(log => log.UserId == userId);

            if (logEntity == null)
            {
                throw new Exception($"Log for user ID {userId} not found.");
            }

            return new Log
            {
                logId = logEntity.LogId,
                userId = (int)logEntity.UserId,
                actionType = logEntity.ActionType,
                timestamp = logEntity.Timestamp
            };
        }

        /// <inheritdoc/>
        public async Task addLogAsync(Log log)
        {
            var logEntity = new LogEntity
            {
                UserId = log.userId,
                ActionType = log.actionType,
                Timestamp = log.timestamp
            };

            dbContext.Logs.Add(logEntity);
            await dbContext.SaveChangesAsync();

            log.logId = logEntity.LogId;
        }

        /// <inheritdoc/>
        public async Task deleteLogAsync(int id)
        {
            var logEntity = await dbContext.Logs.FindAsync(id);
            if (logEntity == null)
            {
                throw new Exception($"Log with ID {id} not found.");
            }

            dbContext.Logs.Remove(logEntity);
            await dbContext.SaveChangesAsync();
        }
    }
}
