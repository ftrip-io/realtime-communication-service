using ftrip.io.framework.Correlation;
using ftrip.io.framework.ExceptionHandling.Extensions;
using ftrip.io.framework.HealthCheck;
using ftrip.io.framework.Installers;
using ftrip.io.framework.Mapping;
using ftrip.io.framework.messaging.Installers;
using ftrip.io.framework.Secrets;
using ftrip.io.framework.Tracing;
using ftrip.io.framework.Validation;
using ftrip.io.rtc_service.Installers;
using ftrip.io.rtc_service.Notifications;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ftrip.io.rtc_service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            InstallerCollection.With(
                new HealthCheckUIInstaller(services),
                new AutoMapperInstaller<Startup>(services),
                new FluentValidationInstaller<Startup>(services),
                new EnviromentSecretsManagerInstaller(services),
                new WebSocketJwtAuthenticationInstaller(services),
                new RabbitMQInstaller<Startup>(services, RabbitMQInstallerType.Publisher | RabbitMQInstallerType.Consumer),
                new SignalRInstaller(services),
                new CorrelationInstaller(services),
                new TracingInstaller(services, (tracingSettings) =>
                {
                    tracingSettings.ApplicationLabel = "rtc";
                    tracingSettings.ApplicationVersion = GetType().Assembly.GetName().Version?.ToString() ?? "unknown";
                    tracingSettings.MachineName = Environment.MachineName;
                })
            ).Install();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(policy => policy
               .WithOrigins(Environment.GetEnvironmentVariable("API_PROXY_URL"))
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials()
            );

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCorrelation();
            app.UseFtripioGlobalExceptionHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHub<NotificationsHub>(NotificationsHub.Endpoint);
            });

            app.UseFtripioHealthCheckUI(Configuration.GetSection(nameof(HealthCheckUISettings)).Get<HealthCheckUISettings>());
        }
    }
}