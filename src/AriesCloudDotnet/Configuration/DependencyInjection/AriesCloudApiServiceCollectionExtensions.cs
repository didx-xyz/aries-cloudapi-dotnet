using AriesCloudAPI.Configuration;
using AriesCloudDotnet.Clients;
using AriesCloudDotnet.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// DI extension methods for adding AriesCloudAPI
    /// </summary>
    public static class AriesCloudAPIServiceCollectionExtensions
    {
        /// <summary>
        /// Creates a builder.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddAriesCloudAPIBuilder(this IServiceCollection services)
        {
            return new AriesCloudAPIBuilder(services);
        }

        /// <summary>
        /// Adds AriesCloudAPI.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        private static IAriesCloudAPIBuilder AddAriesCloudAPI(this IServiceCollection services)
        {
            var builder = services.AddAriesCloudAPIBuilder();

            builder
                .AddRequiredPlatformServices()
                .AddWebClients();

            return builder;
        }

        /// <summary>
        /// Adds AriesCloudAPI.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="setupAction">The setup action.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddAriesCloudAPI(this IServiceCollection services, Action<AriesCloudAPIOptions> setupAction)
        {
            services.Configure(setupAction);
            return services.AddAriesCloudAPI();
        }
         
        /// <summary>
        /// Adds the AriesCloudAPI.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddAriesCloudAPI(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AriesCloudAPIOptions>(configuration.GetSection("AriesCloudAPI"));
            return services.AddAriesCloudAPI();
        }

        private static IAriesCloudAPIBuilder AddRequiredPlatformServices(this IAriesCloudAPIBuilder builder)
        {
            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddOptions();
            builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<AriesCloudAPIOptions>>().Value);
            builder.Services.AddHttpClient();

            return builder;
        }

        /// <summary>
        /// Adds the required web client services.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        private static IAriesCloudAPIBuilder AddWebClients(this IAriesCloudAPIBuilder builder)
        {
            //builder.Services.AddSingleton<AriesCloudAPIOptions>(
            //    resolver => {
            //        var o = resolver.GetRequiredService<IOptions<AriesCloudAPIOptions>>().Value;

            //        return new AriesCloudAPIOptions { BaseUri = o.BaseUri};
            //    }
            //);

            builder.Services.AddScoped<AriesCloudClientFactory>();

            //builder.Services.AddScoped<TenantAdminServiceAgent>();
            //builder.Services.AddScoped<ConnectionServiceAgent>();

            ////TODO: move 
            //builder.Services.AddScoped<IEntityKeyResolver, EntityKeyResolver>();

            return builder;
        }
    }
}