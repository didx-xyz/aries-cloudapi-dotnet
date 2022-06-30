﻿
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AriesCloudAPI.Configuration
{
    /// <summary>
    /// AriesCloudAPI helper class for DI configuration
    /// </summary>
    public class AriesCloudAPIBuilder : IAriesCloudAPIBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AriesCloudAPIBuilder"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <exception cref="System.ArgumentNullException">services</exception>
        public AriesCloudAPIBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        public IServiceCollection Services { get; }
    }
}