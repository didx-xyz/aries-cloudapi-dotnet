using AriesCloudAPI.Core.Interfaces;
using AriesCloudAPI.Core.Options;
using AriesCloudAPI.WebClient.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AriesCloudAPI.WebClient.Clients
{
    public class ConnectionServiceAgent : ServiceAgentBase
    {
        private IStateStore _persistedStateStore;
        private IEntityKeyResolver _entityKeyResolver;

        public ConnectionServiceAgent(
            HttpClient client,
            AriesCloudAPIOptions options,
            IStateStore persistedStateStore,
            IEntityKeyResolver entityKeyResolver
        ) : base(
            client,
            options
        )
        {
            _entityKeyResolver = entityKeyResolver;
            _persistedStateStore = persistedStateStore;
        }

        public async Task<ICollection<Connection>> GetAsync(string tenantKey)
        {
            // resolve entityID to tenantApiKey
            var tenantApiKey = await _entityKeyResolver.ResolveAsync(tenantKey);

            // add authorization header (Tenant)
            var httpRequestHeaders = new Dictionary<string, string>
            {
                { "x-api-key", tenantApiKey }
            };

            return await SendAsync<ICollection<Connection>>(HttpMethod.Get,
                "/generic/connections/",
                null,
                null,
                null,
                null,
                true,
                httpRequestHeaders
            );
        }

        public async Task CreateConnection(string tenantKey1, string tenantKey2) {
            var invitationKey = await CreateInvitationAsync(tenantKey1);
            await AcceptInvitationAsync(tenantKey2, invitationKey);
        }

        public async Task<string> CreateInvitationAsync(string tenantKey)
        {
            // TODO:
            // validate to prevent dupliate tenantKey

            // resolve entityID to tenantApiKey
            var tenantApiKey = await _entityKeyResolver.ResolveAsync(tenantKey);

            // add authorization header (Tenant)
            var httpRequestHeaders = new Dictionary<string, string>
            {
                { "x-api-key", tenantApiKey }
            };

            // send
            var response = await SendAsync<InvitationResult>(HttpMethod.Post,
                "/generic/connections/create-invitation",
                null,
                httpRequestHeaders: httpRequestHeaders
            );

            // generate unique key
            var key = _entityKeyResolver.GenerateKey();

            // persist invitation 
            await _persistedStateStore.StoreAsync(key, response);

            return key;
        }

        public async Task AcceptInvitationAsync(string tenantKey, string invitationKey)
        {
            // TODO:
            // validate to prevent dupliate tenantKey

            // resolve entityID to tenantApiKey
            var tenantApiKey = await _entityKeyResolver.ResolveAsync(tenantKey);

            // add authorization header (Tenant)
            var httpRequestHeaders = new Dictionary<string, string>
            {
                { "x-api-key", tenantApiKey }
            };

            ////TODO: last here
            //return null;

            // get invitation 
            var invitation = await _persistedStateStore.GetAsync<InvitationResult>(invitationKey);
            //if (invitation == null) throw new System.Exception("invalid invitation key");

            var request = new AcceptInvitation
            {
                Alias = "",
                Invitation = new ReceiveInvitationRequest
                {
                    Id = invitation.Invitation.Id,
                    Did = invitation.Invitation.Did,
                    ImageUrl = invitation.Invitation.ImageUrl,
                    Label = invitation.Invitation.Label,
                    RecipientKeys = invitation.Invitation.RecipientKeys,
                    RoutingKeys = invitation.Invitation.RoutingKeys,
                    ServiceEndpoint = invitation.Invitation.ServiceEndpoint,
                    Type = invitation.Invitation.Type,
                    AdditionalProperties = invitation.Invitation.AdditionalProperties
                },
                Use_existing_connection = true
            };

            // send
            var response = await SendAsync<Connection>(HttpMethod.Post,
                "/generic/connections/accept-invitation",
                request,
                httpRequestHeaders: httpRequestHeaders
            );
        }
    }
}
