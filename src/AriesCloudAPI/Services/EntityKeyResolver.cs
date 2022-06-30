using AriesCloudAPI.Core.Interfaces;
using AriesCloudAPI.WebClient.Models;
using System;
using System.Threading.Tasks;

namespace AriesCloudAPI.Services
{
    public class EntityKeyResolver : IEntityKeyResolver
    {
        private IStateStore _persistedStateStore;

        public EntityKeyResolver(IStateStore persistedStateStore) 
        {
            _persistedStateStore = persistedStateStore;
        }

        public string GenerateKey()
        {
           // TODO: generate stronger unique key
           return Guid.NewGuid().ToString();
        }

        public async Task<string> ResolveAsync(string key)
        {
            //TODO: hash key
            var persistedTenant = await _persistedStateStore.GetAsync<CreateTenantResponse>(key);
            if (persistedTenant == null) throw new Exception($"Tenant with key '{key}' not found");

            return persistedTenant.Access_token; 
        }
    }
}
