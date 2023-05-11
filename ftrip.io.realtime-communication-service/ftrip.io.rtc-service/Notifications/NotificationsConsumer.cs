using ftrip.io.notification_service.contracts.Notifications.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ftrip.io.rtc_service.Notifications
{
    public class NotificationsConsumer : IConsumer<NotificationSavedEvent>
    {
        private readonly IHubContext<NotificationsHub> _notificationsHubContext;

        public NotificationsConsumer(IHubContext<NotificationsHub> notificationsHubContext)
        {
            _notificationsHubContext = notificationsHubContext;
        }

        public async Task Consume(ConsumeContext<NotificationSavedEvent> context)
        {
            var notificationSaved = context.Message;

            await _notificationsHubContext.Clients
                .User(notificationSaved.UserId)
                .SendAsync("notification", "You have a new notification.");
        }
    }
}