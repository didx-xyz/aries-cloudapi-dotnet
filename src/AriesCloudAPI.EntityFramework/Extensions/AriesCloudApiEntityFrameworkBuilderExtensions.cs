using AriesCloudApi.EntityFramework.Extensions;
using AriesCloudAPI.Configuration.DependencyInjection.Options;
using AriesCloudAPI.Core.Interfaces;
using AriesCloudAPI.EntityFramework.Contexts;
using AriesCloudAPI.EntityFramework.Interfaces;
using AriesCloudAPI.EntityFramework.Stores;
using Microsoft.EntityFrameworkCore;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add EF database support to AriesCloudAPI.
    /// </summary>
    public static class AriesCloudAPIEntityFrameworkBuilderExtensions
    {
        /// <summary>
        /// Configures EF implementation of IClientStore, IResourceStore, and ICorsPolicyService with AriesCloudAPI.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="storeOptionsAction">The store options action.</param>
        /// <returns></returns>
        //public static IAriesCloudAPIBuilder AddConfigurationStore(
        //    this IAriesCloudAPIBuilder builder,
        //    Action<ConfigurationStoreOptions> storeOptionsAction = null)
        //{
        //    return builder.AddConfigurationStore<ConfigurationDbContext>(storeOptionsAction);
        //}

        /// <summary>
        /// Configures EF implementation of IClientStore, IResourceStore, and ICorsPolicyService with AriesCloudAPI.
        /// </summary>
        /// <typeparam name="TContext">The IConfigurationDbContext to use.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="storeOptionsAction">The store options action.</param>
        /// <returns></returns>
        //public static IAriesCloudAPIBuilder AddConfigurationStore<TContext>(
        //    this IAriesCloudAPIBuilder builder,
        //    Action<ConfigurationStoreOptions> storeOptionsAction = null)
        //    where TContext : DbContext, IConfigurationDbContext
        //{
        //    builder.Services.AddConfigurationDbContext<TContext>(storeOptionsAction);

        //    builder.AddClientStore<ClientStore>();
        //    builder.AddResourceStore<ResourceStore>();
        //    builder.AddCorsPolicyService<CorsPolicyService>();

        //    return builder;
        //}

        /// <summary>
        /// Configures caching for IClientStore, IResourceStore, and ICorsPolicyService with AriesCloudAPI.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        //public static IAriesCloudAPIBuilder AddConfigurationStoreCache(
        //    this IAriesCloudAPIBuilder builder)
        //{
        //    builder.AddInMemoryCaching();

        //    // add the caching decorators
        //    builder.AddClientStoreCache<ClientStore>();
        //    builder.AddResourceStoreCache<ResourceStore>();
        //    builder.AddCorsPolicyCache<CorsPolicyService>();

        //    return builder;
        //}

        /// <summary>
        /// Configures EF implementation of IPersistedGrantStore with AriesCloudAPI.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="storeOptionsAction">The store options action.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddOperationalStore(
            this IAriesCloudAPIBuilder builder,
            Action<OperationalStoreOptions> storeOptionsAction = null)
        {
            return builder.AddOperationalStore<PersistedStateDbContext>(storeOptionsAction);
        }

        /// <summary>
        /// Configures EF implementation of IPersistedGrantStore with AriesCloudAPI.
        /// </summary>
        /// <typeparam name="TContext">The IPersistedGrantDbContext to use.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="storeOptionsAction">The store options action.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddOperationalStore<TContext>(
            this IAriesCloudAPIBuilder builder,
            Action<OperationalStoreOptions> storeOptionsAction = null)
            where TContext : DbContext, IPersistedStateDbContext
        {
            builder.Services.AddOperationalDbContext<TContext>(storeOptionsAction);

            builder.Services.AddTransient<IStateStore, PersistedStateStore>();

            //builder.Services.AddTransient<IPersistedTenantStore, PersistedTenantStore>();
            //builder.Services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();
            //builder.Services.AddTransient<IDeviceFlowStore, DeviceFlowStore>();
            //builder.Services.AddSingleton<IHostedService, TokenCleanupHost>();

            return builder;
        }

        /// <summary>
        /// Adds an implementation of the IOperationalStoreNotification to AriesCloudAPI.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        //public static IAriesCloudAPIBuilder AddOperationalStoreNotification<T>(
        //   this IAriesCloudAPIBuilder builder)
        //   where T : class, IOperationalStoreNotification
        //{
        //    builder.Services.AddOperationalStoreNotification<T>();
        //    return builder;
        //}
    }
}
