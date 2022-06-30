using AriesCloudAPI.Core.Interfaces;
using AriesCloudAPI.Core.Options;
using AriesCloudAPI.WebClient.Commands;
using AriesCloudAPI.WebClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http; 
using System.Threading.Tasks;

namespace AriesCloudAPI.WebClient.Clients
{
    public class TenantAdminServiceAgent : ServiceAgentBase
    {
        private IStateStore _persistedStateStore;
        private IEntityKeyResolver _entityKeyResolver;

        public TenantAdminServiceAgent(
            HttpClient client,
            AriesCloudAPIOptions options,
            IStateStore persistedStateStore,
            IEntityKeyResolver entityKeyResolver
        ) : base(
            client,
            options
        )
        {
            _persistedStateStore = persistedStateStore;
            _entityKeyResolver = entityKeyResolver;
        }

        public async Task<ICollection<Tenant>> GetAsync()
        { 
            // add authorization header (Tenant Admin)
            var httpRequestHeaders = new Dictionary<string, string>
            {
                { "x-api-key", _options.TenantAdminAPIKey }
            }; 

            return await SendAsync<ICollection<Tenant>>(HttpMethod.Get,
                "/admin/tenants",
                null,
                null,
                null,
                null,
                true,
                httpRequestHeaders
            );
        }

        /// <summary>
        /// Creates a new tenant in the Aries Cloud API. 
        /// </summary>
        /// <param name="tenantName"></param>
        /// <param name="imageUrl"></param>
        /// <returns>The unique key identier for the tenant.</returns>
        public async Task<string> CreateAsync(CreateTenantCommand command)
        {
            // TODO:
            // validate to prevent dupliate tenantKey

            // add authorization header (Tenant Admin)
            var httpRequestHeaders = new Dictionary<string, string>
            {
                { "x-api-key", _options.TenantAdminAPIKey }
            };

            // create request
            var request = new CreateTenantRequest { 
                Name = command.Name,
                Image_url = !string.IsNullOrEmpty(command.ImageUrl) ? new Uri(command.ImageUrl) : null,
                Roles = command.Roles?.Length > 0 ? command.Roles.Cast<Roles2>().ToArray() : null
            };

            // send
            var response = await SendAsync<CreateTenantResponse>(HttpMethod.Post,
                "/admin/tenants",
                request, 
                httpRequestHeaders: httpRequestHeaders
            );

            // generate unique key
            var key = _entityKeyResolver.GenerateKey();

            // persist response 
            await _persistedStateStore.StoreAsync(key, response);
             
            return key;
        }
    }

}
