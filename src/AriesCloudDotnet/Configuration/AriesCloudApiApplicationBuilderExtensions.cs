using AriesCloudDotnet.Clients;
using AriesCloudDotnet.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Pipeline extension methods for adding AriesCloudApi
    /// </summary>
    public static class AriesCloudApiApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds AriesCloudAPI to the pipeline.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseAriesCloudAPI(this IApplicationBuilder app)
        {
            app.Validate();

            return app;
        }

        internal static void Validate(this IApplicationBuilder app)
        { 
            var loggerFactory = app.ApplicationServices.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));

            var logger = loggerFactory.CreateLogger("AriesCloudAPI.Startup");
            logger.LogInformation("Starting AriesCloudAPI version {version}");

            var scopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                // validate configuration
                var options = getRequiredService<IOptions<AriesCloudAPIOptions>>(serviceProvider, logger, $"No configuration found. See the 'AddAriesCloudAPI' extension method for configuration options.");

                if (options?.Value == null) {

                    if (string.IsNullOrWhiteSpace(options?.Value.BaseUri))
                        throw new InvalidOperationException($"Invalid 'BaseUri' configuration setting. See the 'AddAriesCloudAPI' extension method for configuration options. Also check the 'AriesCloudAPI' section in your appsettings.json file.");

                    if (string.IsNullOrWhiteSpace(options?.Value.APIKey))
                        throw new InvalidOperationException($"Invalid 'APIKey' configuration setting. See the 'AddAriesCloudAPI' extension method for configuration options. Also check the 'AriesCloudAPI' section in your appsettings.json file.");
                }

                // validate services
                getRequiredService<AriesCloudClientFactory>(serviceProvider,  logger, "No client factory registered. Use the 'AddAriesCloudAPI' extension method to register the required services.");
            }
        } 

        internal static T getRequiredService<T>(IServiceProvider serviceProvider, ILogger logger, string message = null, bool doThrow = true)
        {
            var appService = serviceProvider.GetService<T>();

            if (appService == null)
            {
                var error = message ?? $"Required service {typeof (T).FullName} is not registered in the DI container. Aborting startup";

                logger.LogCritical(error);

                if (doThrow)
                {
                    throw new InvalidOperationException(error);
                }
            }

            return appService;
        }
    }
}
