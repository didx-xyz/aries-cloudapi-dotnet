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
    /// Implementation of IPersistedInvitationStore thats uses EF.
    /// </summary>
    /// <seealso cref="AriesCloudAPI.Stores.IPersistedInvitationStore" />
    public class PersistedInvitationStore : IPersistedInvitationStore
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
        /// Initializes a new instance of the <see cref="PersistedInvitationStore"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        public PersistedInvitationStore(PersistedStateDbContext context, ILogger<PersistedInvitationStore> logger)
        {
            Context = context;
            Logger = logger;
        }

        /// <inheritdoc/>
        public virtual async Task StoreAsync(PersistedInvitation token)
        {
            var existing = (await Context.PersistedInvitations.Where(x => x.Key == token.Key).ToArrayAsync())
                .SingleOrDefault(x => x.Key == token.Key);
            if (existing == null)
            {
                Logger.LogDebug("{persistedInvitationKey} not found in database", token.Key);

                var persistedInvitation = token.ToEntity();
                Context.PersistedInvitations.Add(persistedInvitation);
            }
            else
            {
                Logger.LogDebug("{persistedInvitationKey} found in database", token.Key);

                token.UpdateEntity(existing);
            }

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Logger.LogWarning("exception updating {persistedInvitationKey} persisted Invitation in database: {error}", token.Key, ex.Message);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<PersistedInvitation> GetAsync(string key)
        {
            var persistedInvitation = (await Context.PersistedInvitations.AsNoTracking().Where(x => x.Key == key).ToArrayAsync())
                .SingleOrDefault(x => x.Key == key);
            var model = persistedInvitation?.ToModel();

            Logger.LogDebug("{persistedInvitationKey} found in database: {persistedInvitationKeyFound}", key, model != null);

            return model;
        }

        /// <inheritdoc/>
        //public async Task<IEnumerable<PersistedInvitation>> GetAllAsync(PersistedInvitationFilter filter)
        //{
        //    filter.Validate();

        //    var persistedInvitations = await Filter(Context.PersistedInvitations.AsQueryable(), filter).ToArrayAsync();
        //    persistedInvitations = Filter(persistedInvitations.AsQueryable(), filter).ToArray();
            
        //    var model = persistedInvitations.Select(x => x.ToModel());

        //    Logger.LogDebug("{persistedInvitationCount} persisted Invitations found for {@filter}", persistedInvitations.Length, filter);

        //    return model;
        //}

        /// <inheritdoc/>
        public virtual async Task RemoveAsync(string key)
        {
            var persistedInvitation = (await Context.PersistedInvitations.Where(x => x.Key == key).ToArrayAsync())
                .SingleOrDefault(x => x.Key == key);
            if (persistedInvitation!= null)
            {
                Logger.LogDebug("removing {persistedInvitationKey} persisted Invitation from database", key);

                Context.PersistedInvitations.Remove(persistedInvitation);

                try
                {
                    await Context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException ex)
                {
                    Logger.LogInformation("exception removing {persistedInvitationKey} persisted Invitation from database: {error}", key, ex.Message);
                }
            }
            else
            {
                Logger.LogDebug("no {persistedInvitationKey} persisted Invitation found in database", key);
            }
        }

        /// <inheritdoc/>
        //public async Task RemoveAllAsync(PersistedInvitationFilter filter)
        //{
        //    filter.Validate();

        //    var persistedInvitations = await Filter(Context.PersistedInvitations.AsQueryable(), filter).ToArrayAsync();
        //    persistedInvitations = Filter(persistedInvitations.AsQueryable(), filter).ToArray();

        //    Logger.LogDebug("removing {persistedInvitationCount} persisted Invitations from database for {@filter}", persistedInvitations.Length, filter);

        //    Context.PersistedInvitations.RemoveRange(persistedInvitations);

        //    try
        //    {
        //        await Context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        Logger.LogInformation("removing {persistedInvitationCount} persisted Invitations from database for subject {@filter}: {error}", persistedInvitations.Length, filter, ex.Message);
        //    }
        //}


        //private IQueryable<Entities.PersistedInvitation> Filter(IQueryable<Entities.PersistedInvitation> query, PersistedInvitationFilter filter)
        //{
        //    if (!String.IsNullOrWhiteSpace(filter.ClientId))
        //    {
        //        query = query.Where(x => x.ClientId == filter.ClientId);
        //    }
        //    if (!String.IsNullOrWhiteSpace(filter.SessionId))
        //    {
        //        query = query.Where(x => x.SessionId == filter.SessionId);
        //    }
        //    if (!String.IsNullOrWhiteSpace(filter.SubjectId))
        //    {
        //        query = query.Where(x => x.SubjectId == filter.SubjectId);
        //    }
        //    if (!String.IsNullOrWhiteSpace(filter.Type))
        //    {
        //        query = query.Where(x => x.Type == filter.Type);
        //    }

        //    return query;
        //}
    }
}