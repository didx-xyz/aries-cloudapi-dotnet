using AriesCloudAPI.Core.Interfaces;
using AriesCloudAPI.Core.Models;
using AriesCloudAPI.EntityFramework.Contexts;
using AriesCloudAPI.EntityFramework.Mappers;
using AriesCloudAPI.Extensions;
using AriesCloudAPI.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AriesCloudAPI.EntityFramework.Stores
{
    /// <summary>
    /// Implementation of IPersistedTenantStore thats uses EF.
    /// </summary>
    /// <seealso cref="AriesCloudAPI.Stores.IPersistedTenantStore" />
    public class PersistedTenantStore : IPersistedTenantStore
    {
        /// <summary>
        /// The DbContext.
        /// </summary>
        protected readonly PersistedStateDbContext Context;

        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILogger Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistedTenantStore"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        public PersistedTenantStore(PersistedStateDbContext context, ILogger<PersistedTenantStore> logger)
        {
            Context = context;
            Logger = logger;
        }

        /// <inheritdoc/>
        public virtual async Task StoreAsync(PersistedTenant token)
        {
            var existing = (await Context.PersistedTenants.Where(x => x.Key == token.Key).ToArrayAsync())
                .SingleOrDefault(x => x.Key == token.Key);
            if (existing == null)
            {
                Logger.LogDebug("{persistedTenantKey} not found in database", token.Key);

                var persistedTenant = token.ToEntity();
                Context.PersistedTenants.Add(persistedTenant);
            }
            else
            {
                Logger.LogDebug("{persistedTenantKey} found in database", token.Key);

                token.UpdateEntity(existing);
            }

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Logger.LogWarning("exception updating {persistedTenantKey} persisted Tenant in database: {error}", token.Key, ex.Message);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<PersistedTenant> GetAsync(string key)
        {
            var persistedTenant = (await Context.PersistedTenants.AsNoTracking().Where(x => x.Key == key).ToArrayAsync())
                .SingleOrDefault(x => x.Key == key);
            var model = persistedTenant?.ToModel();

            Logger.LogDebug("{persistedTenantKey} found in database: {persistedTenantKeyFound}", key, model != null);

            return model;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<PersistedTenant>> GetAllAsync(PersistedTenantFilter filter)
        {
            filter.Validate();

            var persistedTenants = await Filter(Context.PersistedTenants.AsQueryable(), filter).ToArrayAsync();
            persistedTenants = Filter(persistedTenants.AsQueryable(), filter).ToArray();
            
            var model = persistedTenants.Select(x => x.ToModel());

            Logger.LogDebug("{persistedTenantCount} persisted Tenants found for {@filter}", persistedTenants.Length, filter);

            return model;
        }

        /// <inheritdoc/>
        public virtual async Task RemoveAsync(string key)
        {
            var persistedTenant = (await Context.PersistedTenants.Where(x => x.Key == key).ToArrayAsync())
                .SingleOrDefault(x => x.Key == key);
            if (persistedTenant!= null)
            {
                Logger.LogDebug("removing {persistedTenantKey} persisted Tenant from database", key);

                Context.PersistedTenants.Remove(persistedTenant);

                try
                {
                    await Context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException ex)
                {
                    Logger.LogInformation("exception removing {persistedTenantKey} persisted Tenant from database: {error}", key, ex.Message);
                }
            }
            else
            {
                Logger.LogDebug("no {persistedTenantKey} persisted Tenant found in database", key);
            }
        }

        /// <inheritdoc/>
        public async Task RemoveAllAsync(PersistedTenantFilter filter)
        {
            filter.Validate();

            var persistedTenants = await Filter(Context.PersistedTenants.AsQueryable(), filter).ToArrayAsync();
            persistedTenants = Filter(persistedTenants.AsQueryable(), filter).ToArray();

            Logger.LogDebug("removing {persistedTenantCount} persisted Tenants from database for {@filter}", persistedTenants.Length, filter);

            Context.PersistedTenants.RemoveRange(persistedTenants);

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Logger.LogInformation("removing {persistedTenantCount} persisted Tenants from database for subject {@filter}: {error}", persistedTenants.Length, filter, ex.Message);
            }
        }


        private IQueryable<Entities.PersistedTenant> Filter(IQueryable<Entities.PersistedTenant> query, PersistedTenantFilter filter)
        {

            //if (!String.IsNullOrWhiteSpace(filter.ClientId))
            //{
            //    query = query.Where(x => x.ClientId == filter.ClientId);
            //}
            //if (!String.IsNullOrWhiteSpace(filter.SessionId))
            //{
            //    query = query.Where(x => x.SessionId == filter.SessionId);
            //}
            //if (!String.IsNullOrWhiteSpace(filter.SubjectId))
            //{
            //    query = query.Where(x => x.SubjectId == filter.SubjectId);
            //}
            //if (!String.IsNullOrWhiteSpace(filter.Type))
            //{
            //    query = query.Where(x => x.Type == filter.Type);
            //}

            return query;
        }
    }
}