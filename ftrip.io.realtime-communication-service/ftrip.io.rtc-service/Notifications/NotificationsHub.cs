using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ftrip.io.rtc_service.Notifications
{
    [Authorize]
    public class NotificationsHub : Hub
    {
        public static string Endpoint = "/hubs/notifications";
    }
}