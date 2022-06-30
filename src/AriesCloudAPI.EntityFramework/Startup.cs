using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace AriesCloudAPI.EntityFramework
{
    public static class Startup
    {
        public static void ConfigureServices_InfrastructureSQLDB(this IServiceCollection services,
           IConfiguration configuration)
        {
            // infrastructure
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SQLDBAssets")));

            // repositories
            services.AddScoped<ISQLRepository<CaptureMethod>, CaptureMethodRepository>();
            services.AddScoped<ISQLRepository<Condition>, ConditionRepository>();
            services.AddScoped<ISQLRepository<Floor>, FloorRepository>();
            services.AddScoped<ISQLRepository<MobileProperty>, MobilePropertyRepository>();
            services.AddScoped<ISQLRepository<CZML>, CZMLRepository>();
            services.AddScoped<ISQLRepository<DeviceCapturer>, DeviceCapturerRepository>();
        }

        public static void Configure_InfrastructureSQLDB(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var logger = scope.ServiceProvider.GetService<ILogger<ApplicationDbContext>>();
                logger.LogDebug("Applying database migrations...");

                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

                // migrate db
                context.Database.Migrate();
            }
        }
    }
}
