// <copyright file="LoggerViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Diagnostics;

namespace WinUI.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using WinUI.Model;
    using WinUI.Service;

    /// <summary>
    /// View model for managing and displaying system logs.
    /// </summary>
    public class LoggerViewModel : BaseViewModel
    {

        private readonly ILoggerService _logger_service;
        private string _user_id_input = string.Empty;
        private ActionType _selected_action_type;
        private DateTime _selected_timestamp = DateTime.Now;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerViewModel"/> class.
        /// </summary>
        /// <param name="logger_service">The logger manager model interface.</param>
        public LoggerViewModel(ILoggerService logger_service)
        {
            this._logger_service = logger_service ?? throw new ArgumentNullException(nameof(logger_service));
            this.logs = new ObservableCollection<LogEntryModel>();
            this.actionTypes = Enum.GetValues(typeof(ActionType)).Cast<ActionType>().ToList();

            // Initialize commands
            this.loadAllLogsCommand = new RelayCommand(async () => await this.executeLoadAllLogsAsync());
            this.filterLogsByUserIdCommand = new RelayCommand(async () => await this.executeFilterLogsByUserIdAsync());
            this.filterLogsByTimestampCommand = new RelayCommand(async () => await this.executeFilterLogsByTimestampAsync());
            this.filterLogsByActionTypeCommand = new RelayCommand(async () => await this.executeFilterLogsByActionTypeAsync());
            this.applyAllFiltersCommand = new RelayCommand(async () => await this.executeApplyAllFiltersAsync());
        }

        /// <summary>
        /// Gets the collection of log entries to display.
        /// </summary>
        public ObservableCollection<LogEntryModel> logs { get; private set; }

        /// <summary>
        /// Gets the command to load all logs.
        /// </summary>
        public ICommand loadAllLogsCommand { get; }

        /// <summary>
        /// Gets the command to filter logs by user ID.
        /// </summary>
        public ICommand filterLogsByUserIdCommand { get; }

        /// <summary>
        /// Gets the command to filter logs by timestamp.
        /// </summary>
        public ICommand filterLogsByTimestampCommand { get; }

        /// <summary>
        /// Gets the command to filter logs by action type.
        /// </summary>
        public ICommand filterLogsByActionTypeCommand { get; }

        /// <summary>
        /// Gets the command to apply all filters simultaneously.
        /// </summary>
        public ICommand applyAllFiltersCommand { get; }

        /// <summary>
        /// Gets the list of available action types for filtering.
        /// </summary>
        public List<ActionType> actionTypes { get; }

        /// <summary>
        /// Gets or sets the user ID input for filtering.
        /// </summary>
        public string user_id_input
        {
            get => this._user_id_input;
            set
            {
                this._user_id_input = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selected action type for filtering.
        /// </summary>
        public ActionType selected_action_type
        {
            get => this._selected_action_type;
            set
            {
                if (this._selected_action_type != value)
                {
                    this._selected_action_type = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected timestamp for filtering.
        /// </summary>
        public DateTime selected_timestamp
        {
            get => this._selected_timestamp;
            set
            {
                if (this._selected_timestamp != value)
                {
                    this._selected_timestamp = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Loads all available logs from the data source.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task executeLoadAllLogsAsync()
        {
            IEnumerable<LogEntryModel> logEntries = await this._logger_service.getAllLogs();
            this.updateLogsCollection(logEntries);
        }

        /// <summary>
        /// Filters logs by the provided user ID.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task executeFilterLogsByUserIdAsync()
        {
            if (string.IsNullOrWhiteSpace(this.user_id_input))
            {
                IEnumerable<LogEntryModel> all_logs = await this._logger_service.getAllLogs();
                this.updateLogsCollection(all_logs);
                return;
            }

            if (int.TryParse(this.user_id_input, out int userId))
            {
                try
                {
                    IEnumerable<LogEntryModel> filtered_logs = await this._logger_service.getLogsByUserId(userId);
                    this.updateLogsCollection(filtered_logs);
                }
                catch (ArgumentException)
                {
                    // Handle invalid user ID gracefully - show all logs instead
                    IEnumerable<LogEntryModel> all_logs = await this._logger_service.getAllLogs();
                    this.updateLogsCollection(all_logs);
                }
            }
        }

        /// <summary>
        /// Filters logs by the selected timestamp.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task executeFilterLogsByTimestampAsync()
        {
            try
            {
                IEnumerable<LogEntryModel> filtered_logs = await this._logger_service.getLogsBeforeTimestamp(this.selected_timestamp);
                this.updateLogsCollection(filtered_logs);
            }
            catch (ArgumentException)
            {
                // Handle invalid timestamp gracefully
                IEnumerable<LogEntryModel> all_logs = await this._logger_service.getAllLogs();
                this.updateLogsCollection(all_logs);
            }
        }

        /// <summary>
        /// Filters logs by the selected action type.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task executeFilterLogsByActionTypeAsync()
        {
            try
            {
                IEnumerable<LogEntryModel> filtered_logs = await this._logger_service.getLogsByActionType(this.selected_action_type);
                this.updateLogsCollection(filtered_logs);
            }
            catch (ArgumentNullException)
            {
                // Handle null action type gracefully
                IEnumerable<LogEntryModel> all_logs = await this._logger_service.getAllLogs();
                this.updateLogsCollection(all_logs);
            }
        }

        /// <summary>
        /// Applies all selected filters simultaneously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task executeApplyAllFiltersAsync()
        {
            int? user_id = null;

            if (int.TryParse(this.user_id_input, out int parsed_user_id))
            {
                user_id = parsed_user_id;
            }

            try
            {
                IEnumerable<LogEntryModel> filtered_logs = await this._logger_service.getLogsWithParameters(user_id, this.selected_action_type, this.selected_timestamp);
                this.updateLogsCollection(filtered_logs);
            }
            catch (Exception)
            {
                // Handle any exceptions gracefully
                IEnumerable<LogEntryModel> all_logs = await this._logger_service.getAllLogs();
                this.updateLogsCollection(all_logs);
            }
        }

        /// <summary>
        /// Updates the logs collection with the provided entries.
        /// </summary>
        /// <param name="_log_entries">The log entries to display.</param>
        private void updateLogsCollection(IEnumerable<LogEntryModel> _log_entries)
        {
            this.logs.Clear();

            if (_log_entries != null)
            {
                foreach (LogEntryModel log_entry in _log_entries)
                {
                    this.logs.Add(log_entry);
                }
            }
        }
    }
}
