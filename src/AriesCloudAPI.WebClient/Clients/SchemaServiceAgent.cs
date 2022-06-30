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
    public class SchemaServiceAgent : ServiceAgentBase
    {
        private IStateStore _persistedStateStore;
        private IEntityKeyResolver _entityKeyResolver;

        public SchemaServiceAgent(
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

        public async Task<ICollection<CredentialSchema>> GetAsync()
        {
            // add authorization header (Goverance)
            var httpRequestHeaders = new Dictionary<string, string>
            {
                { "x-api-key", _options.GoveranceAPIKey }
            };

            return await SendAsync<ICollection<CredentialSchema>>(HttpMethod.Get,
                "/generic/definitions/schemas",
                null,
                null,
                null,
                null,
                true,
                httpRequestHeaders
            );
        }

        /// <summary>
        /// Creates a schema in the Aries Cloud API. 
        /// </summary>
        /// <param name="tenantName"></param>
        /// <param name="imageUrl"></param>
        /// <returns>The unique key identier for the credential schema.</returns>
        public async Task<string> CreateSchemaAsync(CreateSchemaCommand command)
        {
            // TODO:
            // validate to prevent dupliate tenantKey

            // add authorization header (Goverance)
            var httpRequestHeaders = new Dictionary<string, string>
            {
                { "x-api-key", _options.GoveranceAPIKey }
            };

            // create request
            var request = new CreateSchema
            {
                Name = command.Name,
                Version = command.Version,
                Attribute_names = command.Attributes
            };

            // send
            var response = await SendAsync<CredentialSchema>(HttpMethod.Post,
                "/generic/definitions/schemas",
                request,
                httpRequestHeaders: httpRequestHeaders
            );

            // generate unique key
            //var key = _entityKeyResolver.GenerateKey();

            //// persist response 
            //await _persistedStateStore.StoreAsync(key, response);

            return response.Id;
        }

        /// <summary>
        /// Creates a crdential definition in the Aries Cloud API. 
        /// </summary>
        /// <param name="tenantName"></param>
        /// <param name="imageUrl"></param>
        /// <returns>The unique key identier for the credential schema.</returns>
        public async Task<string> CreateCredentialDefinitionAsync(string tenantKey, string schemaId)
        {
            // TODO:
            // validate to prevent dupliate tenantKey

            // resolve entityID to tenantApiKey
            var tenantApiKey = await _entityKeyResolver.ResolveAsync(tenantKey);

            // add authorization header (tenant)
            var httpRequestHeaders = new Dictionary<string, string>
            {
                { "x-api-key", tenantApiKey }
            };

            // create request
            var request = new CreateCredentialDefinition
            {
                Tag = "Default",
                Schema_id = schemaId
            };

            // send
            var response = await SendAsync<CredentialDefinition>(HttpMethod.Post,
                "/generic/definitions/credentials",
                request,
                httpRequestHeaders: httpRequestHeaders
            );

            // generate unique key
            //var key = _entityKeyResolver.GenerateKey();

            //// persist response 
            //await _persistedStateStore.StoreAsync(key, response);

            return response.Id;
        }
    }

}
