using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUI.ViewModel;
using ClassLibrary.Domain;

namespace WinUI.View
{
    public sealed partial class NotificationWindow : Window
    {
        public NotificationViewModel _notification_view_model { get; }

        public NotificationWindow(NotificationViewModel view_model)
        {
            this.InitializeComponent();
            this._notification_view_model = view_model;
            loadNotifications();
        }

        private async void loadNotifications()
        {
            await this._notification_view_model.loadNotificationsAsync(_notification_view_model.userId);
        }

        private async void deleteButtonClick(object sender, RoutedEventArgs routed_event)
        {
            Button button = sender as Button;
            if (button != null)
            {
                Notification notification = button.DataContext as Notification;
                if (notification != null)
                {
                    await this._notification_view_model.deleteNotificationAsync(notification.notificationId, this._notification_view_model.userId);
                }
            }
        }
    }
}