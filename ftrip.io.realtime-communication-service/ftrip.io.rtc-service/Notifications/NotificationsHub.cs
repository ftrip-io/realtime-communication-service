using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using System;
using System.Threading.Tasks;

namespace ftrip.io.rtc_service.Notifications
{
    [Authorize]
    public class NotificationsHub : Hub
    {
        public static string Endpoint = "/hubs/notifications";

        private readonly ILogger _logger;

        public NotificationsHub(ILogger logger)
        {
            _logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            _logger.Information(
                "New connection to the Notifications Hub - ConnectionId[{ConnectionId}], UserId[{UserId}]",
                Context.ConnectionId, Context.UserIdentifier
            );

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _logger.Information(
                "Connection disconnected from the Notifications Hub - ConnectionId[{ConnectionId}], UserId[{UserId}]",
                Context.ConnectionId, Context.UserIdentifier
            );

            return base.OnDisconnectedAsync(exception);
        }
    }
}