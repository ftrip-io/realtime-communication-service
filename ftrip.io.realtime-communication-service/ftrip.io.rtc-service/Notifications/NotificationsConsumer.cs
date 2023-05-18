using ftrip.io.notification_service.contracts.Notifications.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using System.Threading.Tasks;

namespace ftrip.io.rtc_service.Notifications
{
    public class NotificationsConsumer : IConsumer<NotificationSavedEvent>
    {
        private readonly IHubContext<NotificationsHub> _notificationsHubContext;
        private readonly ILogger _logger;

        public NotificationsConsumer(
            IHubContext<NotificationsHub> notificationsHubContext,
            ILogger logger)
        {
            _notificationsHubContext = notificationsHubContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<NotificationSavedEvent> context)
        {
            var notificationSaved = context.Message;

            _logger.Information("Sending a message to the Notification Hub - UserId[{UserId}]", notificationSaved.UserId);

            await _notificationsHubContext.Clients
                .User(notificationSaved.UserId)
                .SendAsync("notification", "You have a new notification.");
        }
    }
}