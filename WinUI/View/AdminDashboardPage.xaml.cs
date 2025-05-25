// <copyright file="AdminDashboardPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUI.View
{
    using System;
    using System.Threading.Tasks;
    using WinUI.Service;
    using WinUI.ViewModel;
    using ClassLibrary.Repository;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Data;
    using Microsoft.UI.Xaml.Navigation;

    /// <summary>
    /// Window for the Admin Dashboard functionality.
    /// </summary>
    public sealed partial class AdminDashboardPage : Page
    {
        private AuthViewModel _auth_view_model;
        private LoggerViewModel _logger_view_model;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminDashboardPage"/> class.
        /// Default constructor for XAML preview.
        /// </summary>
        public AdminDashboardPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminDashboardPage"/> class.
        /// </summary>
        /// <param name="auth_view_model">Authentication service for user operations.</param>
        /// <param name="logger_repository">Logger service for auditing.</param>
        /// <exception cref="ArgumentNullException">Thrown if auth service is null.</exception>
        public AdminDashboardPage(AuthViewModel auth_view_model, ILogRepository logger_repository)
        {
            this.InitializeComponent();
            this._auth_view_model = auth_view_model ?? throw new ArgumentNullException(nameof(auth_view_model));
            initializeLogger(logger_repository ?? throw new ArgumentNullException(nameof(logger_repository)));
        }

        /// <summary>
        /// Override OnNavigatedTo to handle parameters passed during navigation
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs navigation_event)
        {
            base.OnNavigatedTo(navigation_event);

            if (navigation_event.Parameter is Tuple<AuthViewModel, ILogRepository> parameters)
            {
                this._auth_view_model = parameters.Item1;
                initializeLogger(parameters.Item2);
            }
            else if (navigation_event.Parameter is ValueTuple<AuthViewModel, ILogRepository> value_tuple)
            {
                this._auth_view_model = value_tuple.Item1;
                initializeLogger(value_tuple.Item2);
            }
        }

        private void initializeLogger(ILogRepository logger_repository)
        {
            LoggerService logger_manager_model = new LoggerService(logger_repository);
            this._logger_view_model = new LoggerViewModel(logger_manager_model);

            this.loadInitialLogData();

            this.configureUserInterface();
        }

        private void loadInitialLogData()
        {
            this._logger_view_model.loadAllLogsCommand.Execute(null);
        }

        private void configureUserInterface()
        {
            // Set the item source for ListView
            this.LogListView.ItemsSource = this._logger_view_model.logs;

            // Set up ComboBox for action types
            this.ActionTypeComboBox.ItemsSource = this._logger_view_model.actionTypes;

            // Bind TextBox for user ID filtering
            this.UserIdTextBox.SetBinding(TextBox.TextProperty, new Binding
            {
                Path = new PropertyPath("user_id_input"),
                Source = this._logger_view_model,
                Mode = BindingMode.TwoWay,
            });

            // Bind ComboBox for action type filtering
            this.ActionTypeComboBox.SetBinding(ComboBox.SelectedItemProperty, new Binding
            {
                Path = new PropertyPath("selected_action_type"),
                Source = this._logger_view_model,
                Mode = BindingMode.TwoWay,
            });

            // Bind DatePicker for timestamp filtering
            this.TimestampDatePicker.SetBinding(DatePicker.DateProperty, new Binding
            {
                Path = new PropertyPath("selected_timestamp"),
                Source = this._logger_view_model,
                Mode = BindingMode.TwoWay,
                Converter = new Helpers.DateTimeToDateTimeOffsetConverter(),
            });

            // Bind the buttons directly to commands in the ViewModel
            this.LoadAllLogsButton.Command = this._logger_view_model.loadAllLogsCommand;
            this.LoadLogsByUserIdButton.Command = this._logger_view_model.filterLogsByUserIdCommand;
            this.LoadLogsByActionTypeButton.Command = this._logger_view_model.filterLogsByActionTypeCommand;
            this.LoadLogsBeforeTimestampButton.Command = this._logger_view_model.filterLogsByTimestampCommand;
            this.ApplyFiltersButton.Command = this._logger_view_model.applyAllFiltersCommand;
        }

        private async void logoutButtonClick(object sender, RoutedEventArgs @event)
        {
            await this.performLogoutAsync();
        }

        private async Task performLogoutAsync()
        {
            try
            {
                await this._auth_view_model.logout();
                NavigationService.navigateToLogin();
            }
            catch (Exception exception)
            {
                await this.displayErrorDialogAsync($"Logout error: {exception.Message}");
            }
        }


        private async Task displayErrorDialogAsync(string error_message)
        {
            ContentDialog errorDialog = new ContentDialog
            {
                Title = "Error",
                Content = error_message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot,
            };

            await errorDialog.ShowAsync();
        }
    }
}
