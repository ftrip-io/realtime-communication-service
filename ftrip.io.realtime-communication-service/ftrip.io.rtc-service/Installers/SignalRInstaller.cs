using ftrip.io.framework.Installers;
using ftrip.io.rtc_service.Providers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace ftrip.io.rtc_service.Installers
{
    public class SignalRInstaller : IInstaller
    {
        private readonly IServiceCollection _services;

        public SignalRInstaller(IServiceCollection services)
        {
            _services = services;
        }

        public void Install()
        {
            _services.AddSignalR();
            _services.AddSingleton<IUserIdProvider, NameUserIdProvider>();
        }
    }
}