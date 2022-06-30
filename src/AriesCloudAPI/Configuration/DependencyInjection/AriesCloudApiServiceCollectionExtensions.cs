
using AriesCloudAPI.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using AriesCloudAPI.Core.Options;
//using Microsoft.AspNetCore.Authentication.OpenIdConnect;

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
                //.AddCookieAuthentication()
                //.AddCoreServices()
                //.AddDefaultEndpoints()
                //.AddPluggableServices()
                //.AddValidators()
                //.AddResponseGenerators()
                //.AddDefaultSecretParsers()
                //.AddDefaultSecretValidators();

            // provide default in-memory implementation, not suitable for most production scenarios
            //builder.AddInMemoryPersistedGrants();

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

        /// <summary>
        /// Configures the OpenIdConnect handlers to persist the state parameter into the server-side IDistributedCache.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="schemes">The schemes to configure. If none provided, then all OpenIdConnect schemes will use the cache.</param>
        //public static IServiceCollection AddOidcStateDataFormatterCache(this IServiceCollection services, params string[] schemes)
        //{
        //    services.AddSingleton<IPostConfigureOptions<OpenIdConnectOptions>>(
        //        svcs => new ConfigureOpenIdConnectOptions(
        //            schemes,
        //            svcs.GetRequiredService<IHttpContextAccessor>())
        //    );

        //    return services;
        //}
    }
}