using AriesCloudAPI.Core.Interfaces;
using AriesCloudAPI.EntityFramework.Contexts;
using AriesCloudAPI.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AriesCloudAPI.EntityFramework.Stores
{
    /// <summary>
    /// Implementation of IStateStore thats uses EF.
    /// </summary>
    /// <seealso cref="AriesCloudAPI.Stores.IStateStore" />
    public class PersistedStateStore : IStateStore
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
        /// Initializes a new instance of the <see cref="PersistedStateStore"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        public PersistedStateStore(PersistedStateDbContext context, ILogger<PersistedStateStore> logger)
        {
            Context = context;
            Logger = logger;
        }
         
        public virtual async Task StoreAsync(string key, object data)
        {
            var existing = await Context.PersistedStates.FirstOrDefaultAsync(x => x.Key == key);
            if (existing == null)
            {
                Logger.LogDebug("{key} not found in database", key);

                var entity = new PersistedState
                {
                    Key = key,
                    Data = Newtonsoft.Json.JsonConvert.SerializeObject(data)
                };

                Context.PersistedStates.Add(entity);
            }
            else
            {
                Logger.LogDebug("{key} found in database", key);

                existing.Data = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            }

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Logger.LogWarning("exception updating {key} persisted State in database: {error}", key, ex.Message);
            }
        }

        public virtual async Task<T> GetAsync<T>(string key)
        {
            var entity = await Context.PersistedStates.AsNoTracking().FirstOrDefaultAsync(x => x.Key == key);

            Logger.LogDebug("{persistedStateKey} found in database: {persistedStateKeyFound}", key, entity != null);

            if (entity == null) throw new Exception($"{key} not found in database");

            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(entity.Data);
            if (model == null) throw new Exception($"error serializing data for {key}");

            return model;
        }

        //public async Task<IEnumerable<PersistedState>> GetAllAsync(PersistedStateFilter filter)
        //{
        //    filter.Validate();

        //    var persistedStates = await Filter(Context.PersistedStates.AsQueryable(), filter).ToArrayAsync();
        //    persistedStates = Filter(persistedStates.AsQueryable(), filter).ToArray();

        //    var model = persistedStates.Select(x => x.ToModel());

        //    Logger.LogDebug("{persistedStateCount} persisted States found for {@filter}", persistedStates.Length, filter);

        //    return model;
        //}

        public virtual async Task RemoveAsync(string key)
        {
            var persistedState = await Context.PersistedStates.FirstOrDefaultAsync(x => x.Key == key);
            if (persistedState != null)
            {
                Logger.LogDebug("removing {persistedStateKey} persisted State from database", key);

                Context.PersistedStates.Remove(persistedState);

                try
                {
                    await Context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Logger.LogInformation("exception removing {persistedStateKey} persisted State from database: {error}", key, ex.Message);
                }
            }
            else
            {
                Logger.LogDebug("no {persistedStateKey} persisted State found in database", key);
            }
        }

        //public async Task RemoveAllAsync(PersistedStateFilter filter)
        //{
        //    filter.Validate();

        //    var persistedStates = await Filter(Context.PersistedStates.AsQueryable(), filter).ToArrayAsync();
        //    persistedStates = Filter(persistedStates.AsQueryable(), filter).ToArray();

        //    Logger.LogDebug("removing {persistedStateCount} persisted States from database for {@filter}", persistedStates.Length, filter);

        //    Context.PersistedStates.RemoveRange(persistedStates);

        //    try
        //    {
        //        await Context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        Logger.LogInformation("removing {persistedStateCount} persisted States from database for subject {@filter}: {error}", persistedStates.Length, filter, ex.Message);
        //    }
        //}


        //private IQueryable<Entities.PersistedState> Filter(IQueryable<Entities.PersistedState> query, PersistedStateFilter filter)
        //{
        //    //TODO:
        //    //if (!String.IsNullOrWhiteSpace(filter.ClientId))
        //    //{
        //    //    query = query.Where(x => x.ClientId == filter.ClientId);
        //    //}
        //    //if (!String.IsNullOrWhiteSpace(filter.SessionId))
        //    //{
        //    //    query = query.Where(x => x.SessionId == filter.SessionId);
        //    //}
        //    //if (!String.IsNullOrWhiteSpace(filter.SubjectId))
        //    //{
        //    //    query = query.Where(x => x.SubjectId == filter.SubjectId);
        //    //}
        //    //if (!String.IsNullOrWhiteSpace(filter.Type))
        //    //{
        //    //    query = query.Where(x => x.Type == filter.Type);
        //    //}

        //    return query;
        //}
    }
}