using AriesCloudAPI.Core.Interfaces;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace AriesCloudAPI.Stores
{
    /// <summary>
    /// In-memory state store
    /// </summary>
    public class InMemoryStateStore : IStateStore
    {
        private readonly ConcurrentDictionary<string, string> _repository = new ConcurrentDictionary<string, string>();

        /// <inheritdoc/>
        public Task StoreAsync(string key, object data)
        {
            _repository[key] = Newtonsoft.Json.JsonConvert.SerializeObject(data);

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task<T> GetAsync<T>(string key)
        {
            if (_repository.TryGetValue(key, out string data))
            {
                return Task.FromResult(Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data));
            }

            return Task.FromResult<T>(default(T));
        }

        /// <inheritdoc/>
        //public Task<IEnumerable<PersistedTenant>> GetAllAsync(PersistedTenantFilter filter)
        //{
        //    filter.Validate();
            
        //    var items = Filter(filter);
            
        //    return Task.FromResult(items);
        //}

        /// <inheritdoc/>
        public Task RemoveAsync(string key)
        {
            _repository.TryRemove(key, out _);

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        //public Task RemoveAllAsync(PersistedTenantFilter filter)
        //{
        //    filter.Validate();

        //    var items = Filter(filter);
            
        //    foreach (var item in items)
        //    {
        //        _repository.TryRemove(item.Key, out _);
        //    }

        //    return Task.CompletedTask;
        //}

        //private IEnumerable<PersistedTenant> Filter(PersistedTenantFilter filter)
        //{
        //    var query =
        //        from item in _repository
        //        select item.Value;

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

        //    var items = query.ToArray().AsEnumerable();
        //    return items;
        //}
    }
}