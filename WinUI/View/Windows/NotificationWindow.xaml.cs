using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUI.ViewModel;
using ClassLibrary.Domain;

namespace WinUI.View
{
    public sealed partial class NotificationWindow : Window
    {
        public NotificationViewModel _view_model { get; }

        public NotificationWindow(NotificationViewModel _view_model)
        {
            this.InitializeComponent();
            this._view_model = _view_model;
            LoadNotifications();
        }

        private async void LoadNotifications()
        {
            await this._view_model.loadNotificationsAsync(_view_model._user_id);
        }

        private async void DeleteButton_Click(object _sender, RoutedEventArgs _error)
        {
            Button _button = _sender as Button;
            if (_button != null)
            {
                Notification _notification = _button.DataContext as Notification;
                if (_notification != null)
                {
                    await this._view_model.deleteNotificationAsync(_notification._notification_id, this._view_model._user_id);
                }
            }
        }
    }
}