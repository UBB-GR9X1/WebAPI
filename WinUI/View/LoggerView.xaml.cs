// <copyright file="LoggerView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUI.View
{
    using System;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Data;
    using Microsoft.UI.Xaml.Navigation;
    using WinUI.Helpers;
    using ClassLibrary.IRepository;
    using WinUI.Service;
    using WinUI.ViewModel;

    /// <summary>
    /// View for displaying and managing system logs.
    /// </summary>
    public sealed partial class LoggerView : Page
    {
        private LoggerViewModel _logger_view_model;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerView"/> class.
        /// Default constructor required for XAML preview.
        /// </summary>
        public LoggerView()
        {
            this.InitializeComponent();
        }


        /// <summary>
        /// Overrides the OnNavigatedTo method to initialize with parameters.
        /// </summary>
        /// <param name="navigation_event">Navigation event arguments</param>
        protected override void OnNavigatedTo(NavigationEventArgs navigation_event)
        {
            base.OnNavigatedTo(navigation_event);

            // Check if we received a repository in navigation
            if (navigation_event.Parameter is ILogRepository logger_repository)
            {
                initializeWithRepository(logger_repository);
            }
        }

        /// <summary>
        /// Secondary constructor for direct initialization with repository
        /// </summary>
        /// <param name="logger_repository">The logger repository to use</param>
        public LoggerView(ILogRepository logger_repository)
        {
            this.InitializeComponent();
            initializeWithRepository(logger_repository);
        }

        private void initializeWithRepository(ILogRepository logger_repository)
        {
            LoggerService logger_service = new LoggerService(logger_repository);
            this._logger_view_model = new LoggerViewModel(logger_service);

            this.bindUserInterface();
            this.loadInitialLogs();
        }


        private void loadInitialLogs()
        {
            this._logger_view_model.loadAllLogsCommand.Execute(null);
        }

        private void bindUserInterface()
        {
            this.LogListView.ItemsSource = this._logger_view_model.logs;

            this.LoadAllLogsButton.Command = this._logger_view_model.loadAllLogsCommand;

            this.LoadLogsByUserIdButton.Command = this._logger_view_model.filterLogsByUserIdCommand;
            this.UserIdTextBox.DataContext = this._logger_view_model;

            this.LoadLogsByActionTypeButton.Command = this._logger_view_model.filterLogsByActionTypeCommand;
            this.ActionTypeComboBox.ItemsSource = this._logger_view_model.actionTypes;

            this.LoadLogsBeforeTimestampButton.Command = this._logger_view_model.filterLogsByTimestampCommand;

            this.LoadLogsWithAllParametersButton.Command = this._logger_view_model.applyAllFiltersCommand;

            // Bind TextBox, ComboBox, and DatePicker to ViewModel properties
            this.UserIdTextBox.SetBinding(Microsoft.UI.Xaml.Controls.TextBox.TextProperty, new Microsoft.UI.Xaml.Data.Binding
            {
                Path = new Microsoft.UI.Xaml.PropertyPath("user_id_input"),
                Mode = Microsoft.UI.Xaml.Data.BindingMode.TwoWay
            });

            this.ActionTypeComboBox.SetBinding(Microsoft.UI.Xaml.Controls.ComboBox.SelectedItemProperty, new Microsoft.UI.Xaml.Data.Binding
            {
                Source = this._logger_view_model,
                Path = new Microsoft.UI.Xaml.PropertyPath("selected_action_type"),
                Mode = Microsoft.UI.Xaml.Data.BindingMode.TwoWay
            });

            this.TimestampDatePicker.SetBinding(Microsoft.UI.Xaml.Controls.DatePicker.DateProperty, new Microsoft.UI.Xaml.Data.Binding
            {
                Source = this._logger_view_model,
                Path = new Microsoft.UI.Xaml.PropertyPath("selected_timestamp"),
                Mode = Microsoft.UI.Xaml.Data.BindingMode.TwoWay,
                Converter = new DateTimeToDateTimeOffsetConverter()
            });

        }
    }
}