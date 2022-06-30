using AriesCloudAPI.Core.Interfaces;
using AriesCloudAPI.Core.Options;
using AriesCloudAPI.WebClient.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AriesCloudAPI.WebClient.Clients
{
    public class CredentialServiceAgent : ServiceAgentBase
    {
        private IStateStore _persistedStateStore;
        private IEntityKeyResolver _entityKeyResolver;

        public CredentialServiceAgent(
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

        //public async Task<ICollection<CredentialDefinition>> GetAsync(string tenantKey)
        //{
        //    // resolve entityID to tenantApiKey
        //    var tenantApiKey = await _entityKeyResolver.ResolveAsync(tenantKey);

        //    // add authorization header (Tenant)
        //    var httpRequestHeaders = new Dictionary<string, string>
        //    {
        //        { "x-api-key", tenantApiKey }
        //    };

        //    return await SendAsync<ICollection<CredentialDefinition>>(HttpMethod.Get,
        //        "/generic/definitions/credentials/",
        //        null,
        //        null,
        //        null,
        //        null,
        //        true,
        //        httpRequestHeaders
        //    );
        //}

        public async Task<string> IssueAsync(string tenantKey, string tenantKey2, string credentialDefinitionId, Dictionary<string, string> attributes)
        {
            // TODO:
            // validate to prevent dupliate tenantKey

            // todo: lookup tenant connection id

            // resolve entityID to tenantApiKey
            var tenantApiKey = await _entityKeyResolver.ResolveAsync(tenantKey);

            // add authorization header (Tenant)
            var httpRequestHeaders = new Dictionary<string, string>
            {
                { "x-api-key", tenantApiKey }
            };

            // create request
            var request = new SendCredential { 
                Protocol_version = IssueCredentialProtocolVersion.V1,
                Connection_id = "",
                Credential_definition_id = credentialDefinitionId,
                Attributes = attributes
            };

            // send
            var response = await SendAsync<CredentialDefinition>(HttpMethod.Post,
                "/generic/issuer/credentials",
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
                //Use_existing_Credential = true
            };

            // send
            var response = await SendAsync<AcceptInvitation>(HttpMethod.Post,
                "/generic/Credentials/accept-invitation",
                request,
                httpRequestHeaders: httpRequestHeaders
            );
        }
    }
}
