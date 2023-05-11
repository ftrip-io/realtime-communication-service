using ftrip.io.framework.auth;
using ftrip.io.rtc_service.Notifications;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace ftrip.io.rtc_service.Installers
{
    public class WebSocketJwtAuthenticationInstaller : JwtAuthenticationInstaller
    {
        public WebSocketJwtAuthenticationInstaller(IServiceCollection services) :
            base(services)
        {
        }

        protected override void AdditionallyConfigureJwtBearer(JwtBearerOptions options)
        {
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var path = context.HttpContext.Request.Path;
                    if (!path.StartsWithSegments(NotificationsHub.Endpoint))
                    {
                        return Task.CompletedTask;
                    }

                    var accessToken = context.Request.Query["access_token"];
                    if (string.IsNullOrEmpty(accessToken))
                    {
                        return Task.CompletedTask;
                    }

                    context.Token = accessToken;
                    return Task.CompletedTask;
                }
            };
        }
    }
}