using Microsoft.AspNetCore.SignalR;

namespace ftrip.io.rtc_service.Providers
{
    public class NameUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Identity?.Name;
        }
    }
}