using AriesCloudAPI.Clients.AriesCloud.Models;
using AriesCloudDotnet.Configuration;
using System.Web;
using System.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;

namespace AriesCloudDotnet.Clients
{
    public class AriesCloudClient : ServiceBase
    {
        //private ServiceBase _serviceBase;

        //public AriesCloudClient(ServiceBase serviceBase)  
        //{
        //    _serviceBase = serviceBase;
        //}

        //private HttpClient _client;
        private AriesCloudAPIOptions _options;

        public AriesCloudClient(/*HttpClient client,*/ AriesCloudAPIOptions options, Dictionary<string, string> httpRequestHeaders)
            : base(/*client,*/ options.BaseUri, httpRequestHeaders: httpRequestHeaders)
        {

            //_client = client;
            _options = options;
        }

        #region Connections

        /// <summary>
        /// Create connection invitation out-of-band
        /// </summary>
        public async Task<InvitationRecord> CreateOOBInvitationAsync(CreateOobInvitation request)
        {
            return await SendAsync<InvitationRecord>(HttpMethod.Post,
                "/generic/connections/oob/create-invitation",
                request
            );
        }

        /// <summary>
        /// Receive out-of-band invitation
        /// </summary>
        public async Task<Connection> AcceptOOBInvitationAsync(AcceptOobInvitation request)
        {
            return await SendAsync<Connection>(HttpMethod.Post,
                "/generic/connections/oob/accept-invitation",
                request
            );
        }

        /// <summary>
        /// Use a public DID as implicit invitation and connect
        /// </summary>
        public async Task<Connection> ConnectPublicDidAsync(ConnectToPublicDid request)
        {
            return await SendAsync<Connection>(HttpMethod.Post,
                "/generic/connections/oob/connect-public-did",
                request
            );
        }

        /// <summary>
        /// Create connection invitation
        /// </summary>
        public async Task<InvitationResult> CreateInvitationAsync(CreateInvitation request)
        {
            return await SendAsync<InvitationResult>(HttpMethod.Post,
                "/generic/connections/create-invitation",
                request
            );
        }

        /// <summary>
        /// Accept connection invitation
        /// </summary>
        public async Task<Connection> AcceptInvitationAsync(AcceptInvitation request)
        {
            return await SendAsync<Connection>(HttpMethod.Post,
                "/generic/connections/accept-invitation",
                request
            );
        }

        /// <summary>
        /// Retrieve list of connections
        /// </summary>
        public async Task<ICollection<Connection>> GetConnectionsAsync()
        {
            return await SendAsync<ICollection<Connection>>(HttpMethod.Get, "/generic/connections/");
        }

        /// <summary>
        /// Retrieve connection by id
        /// </summary>
        public async Task<Connection> GetConnectionAsync(string connection_id)
        {
            return await SendAsync<Connection>(HttpMethod.Get, $"/generic/connections/{connection_id}");
        }

        /// <summary>
        /// Delete connection by id
        /// </summary>
        public async Task DeleteConnectionAsync(string connection_id)
        {
            await SendAsync(HttpMethod.Delete, $"/generic/connections/{connection_id}");
        }

        #endregion Connections

        #region Issuer

        /// <summary>
        /// Retrieve list of credentials
        /// </summary>
        /// TODO: confirm return type 
        public async Task<ICollection<object>> GetCredentialsAsync(string? connection_id = null)
        {
            return await SendAsync<ICollection<object>>(HttpMethod.Get,
                string.IsNullOrWhiteSpace(connection_id) ? "/generic/issuer/credentials" : $"/generic/issuer/credentials?connection_id={connection_id}");
        }

        /// <summary>
        /// Retrieve credential by id
        /// </summary>
        /// TODO: confirm return type 
        public async Task<object> GetCredentialAsync(string credential_id)
        {
            return await SendAsync<object>(HttpMethod.Get, $"/generic/issuer/credentials/{credential_id}");
        }

        /// <summary>
        /// Create and send credential. Automating the entire flow.
        /// </summary>
        /// TODO: confirm return type 
        public async Task<object> SendCredentialAsync(SendCredential request)
        {
            return await SendAsync<object>(HttpMethod.Post, "/generic/issuer/credentials", request);
        }

        
        public async Task DeleteCredentialAsync(string credential_id)
        {
            await SendAsync(HttpMethod.Delete, $"/generic/issuer/credentials/{credential_id}");
        }

        /// <summary>
        /// Send credential request
        /// </summary>
        /// TODO: confirm return type 
        public async Task<object> RequestCredentialAsync(string credential_id)
        {
            return await SendAsync<object>(HttpMethod.Post, $"/generic/issuer/credentials/{credential_id}/request");
        }

        /// <summary>
        /// Store credential
        /// </summary>
        /// TODO: confirm return type 
        public async Task<object> StoreCredentialAsync(string credential_id)
        {
            return await SendAsync<object>(HttpMethod.Post, $"/generic/issuer/credentials/{credential_id}/store");
        }

        #endregion Issuer

        #region Messaging

        /// <summary>
        /// Send messages
        /// </summary> 
        /// TODO: confirm return type 
        public async Task SendMessageAsync(Message request)
        {
            await SendAsync(HttpMethod.Post, $"/generic/messaging/send-message", request);
        }

        /// <summary>
        /// Trust ping
        /// </summary> 
        public async Task<PingRequestResponse> TrustPingMessageAsync(TrustPingMsg request)
        {
            return await SendAsync<PingRequestResponse>(HttpMethod.Post, $"/generic/messaging/trust-ping", request);
        }
        #endregion Messaging

        #region Wallet

        /// <summary>
        /// Retrieve list of DIDs
        /// </summary>
        public async Task<ICollection<DID>> GetDidsAsync()
        {
            return await SendAsync<ICollection<DID>>(HttpMethod.Get, "/wallet/dids/");
        }

        /// <summary>
        /// Create Local DID
        /// </summary> 
        public async Task<DID> CreateDidAsync()
        {
            return await SendAsync<DID>(HttpMethod.Post, $"/wallet/dids");
        }

        /// <summary>
        /// Fetch the current public DID
        /// </summary>
        public async Task<DID> GetPublicDidAsync()
        {
            return await SendAsync<DID>(HttpMethod.Get, "/wallet/dids/public");
        }

        /// <summary>
        /// Set the current public DID
        /// </summary> 
        public async Task<DID> SetPublicDidAsync(string did)
        {
            return await SendAsync<DID>(HttpMethod.Put, $"/wallet/dids/public?did={did}");
        }

        /// <summary>
        /// Rotate the keypair
        /// </summary> 
        public async Task RotateKeypairAsync(string did)
        {
            await SendAsync(HttpMethod.Patch, $"/wallet/dids/{did}/rotate-keypair");
        }

        /// <summary>
        /// Get DID endpoint
        /// </summary> 
        public async Task<DIDEndpoint> GetDidEndpointAsync(string did)
        {
            return await SendAsync<DIDEndpoint>(HttpMethod.Get, $"/wallet/dids/{did}/endpoint");
        }

        /// <summary>
        /// Set DID endpoint
        /// </summary> 
        public async Task SetDidEndpointAsync(string did, SetDidEndpointRequest request)
        {
            await SendAsync(HttpMethod.Post, $"/wallet/dids/{did}/endpoint", request);
        }

        #endregion Wallet

        #region Tenants

        /// <summary>
        /// Get tenants
        /// </summary>
        public async Task<ICollection<Tenant>> GetTenantsAsync()
        {
            return await SendAsync<ICollection<Tenant>>(HttpMethod.Get, "/admin/tenants/");
        }

        /// <summary>
        /// Create a new tenant
        /// </summary> 
        public async Task<CreateTenantResponse> CreateTenantAsync(CreateTenantRequest request)
        {
            return await SendAsync<CreateTenantResponse>(HttpMethod.Post, "/admin/tenants/", request);
        }

        /// <summary>
        /// Get tenant by id
        /// </summary> 
        public async Task<Tenant> GetTenantAsync(string tenant_id)
        {
            return await SendAsync<Tenant>(HttpMethod.Get, $"/admin/tenants/{tenant_id}");
        }

        /// <summary>
        /// Update tenant by id
        /// </summary> 
        public async Task<Tenant> UpdateTenantAsync(string tenant_id, UpdateTenantRequest request)
        {
            return await SendAsync<Tenant>(HttpMethod.Put, $"/admin/tenants/{tenant_id}", request);
        }

        /// <summary>
        /// Delete tenant by id
        /// </summary> 
        public async Task DeleteTenantAsync(string tenant_id)
        {
            await SendAsync(HttpMethod.Delete, $"/admin/tenants/{tenant_id}");
        }

        /// <summary>
        /// Get tenant auth token
        /// </summary> 
        //public async Task<TenantAuth> GetTenantAuthTokenAsync(string tenant_id)
        //{
        //    return await SendAsync<TenantAuth>(HttpMethod.Get, $"/admin/tenants/{tenant_id}/access-token");
        //}

        #endregion Tenants

        #region Verifier

        /// <summary>
        /// Get matching credentials for presentation exchange
        /// </summary>
        /// TODO: confirm return type 
        public async Task<object> GetCredentialsForRequestAsync(string proof_id)
        {
            return await SendAsync<object>(HttpMethod.Get, $"/generic/verifier/proofs/{proof_id}/credentials");
        }

        /// <summary>
        /// Get all proof records
        /// </summary>
        /// TODO: confirm return type 
        public async Task<object> GetProofRecordsAsync()
        {
            return await SendAsync<object>(HttpMethod.Get, $"/generic/verifier/proofs/");
        }

        /// <summary>
        /// Get a specific proof record
        /// </summary>
        /// TODO: confirm return type 
        public async Task<object> GetProofRecordAsync(string proof_id)
        {
            return await SendAsync<object>(HttpMethod.Get, $"/generic/verifier/proofs/{proof_id}");
        }

        /// <summary>
        /// Delete proofs record for proof_id (pres_ex_id including prepending version hint 'v1-' or 'v2-')
        /// </summary> 
        public async Task DeleteProofAsync(string proof_id)
        {
            await SendAsync(HttpMethod.Delete, $"/generic/verifier/proofs/{proof_id}");
        }

        /// <summary>
        /// Send proof request
        /// </summary>
        /// TODO: confirm return type
        public async Task<object> SendProofRequestAsync(SendProofRequest request)
        {
            return await SendAsync<object>(HttpMethod.Post, $"/generic/verifier/send-request", request);
        }

        /// <summary>
        /// Create proof request
        /// </summary>
        /// TODO: confirm return type
        public async Task<object> CreateProofRequestAsync(CreateProofRequest request)
        {
            return await SendAsync<object>(HttpMethod.Post, $"/generic/verifier/create-request", request);
        }

        /// <summary>
        /// Accept proof request
        /// </summary>
        /// TODO: confirm return type
        public async Task<object> AcceptProofRequestAsync(AcceptProofRequest request)
        {
            return await SendAsync<object>(HttpMethod.Post, $"/generic/verifier/accept-request", request);
        }

        /// <summary>
        /// Reject proof request
        /// </summary>
        /// TODO: confirm return type
        public async Task<object> RejectProofRequestAsync(RejectProofRequest request)
        {
            return await SendAsync<object>(HttpMethod.Post, $"/generic/verifier/reject-request", request);
        }

        #endregion Verifier

        #region Trust Registry

        /// <summary>
        /// Get the trust registry
        /// </summary> 
        public async Task<TrustRegistry> GetTrustRegistryAsync()
        {
            return await SendAsync<TrustRegistry>(HttpMethod.Get, $"/trust-registry");
        }

        #endregion Trust Registry

        #region Definitions

        /// <summary>
        /// Retrieve credential definitions the current agent created
        /// </summary> 
        public async Task<ICollection<CredentialDefinition>> GetCredentialDefinitionsAsync(
            string? issuer_did = null,
            string? credential_definition_id = null,
            string? schema_id = null,
            string? schema_issuer_did = null,
            string? schema_name = null,
            string? schema_version = null)
        {
            // construct querystring 
            var args = HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrWhiteSpace(issuer_did)) args.Add("issuer_did", issuer_did);
            if (!string.IsNullOrWhiteSpace(credential_definition_id)) args.Add("credential_definition_id", credential_definition_id);
            if (!string.IsNullOrWhiteSpace(schema_id)) args.Add("schema_id", schema_id);
            if (!string.IsNullOrWhiteSpace(schema_issuer_did)) args.Add("schema_issuer_did", schema_issuer_did);
            if (!string.IsNullOrWhiteSpace(schema_name)) args.Add("schema_name", schema_name);
            if (!string.IsNullOrWhiteSpace(schema_version)) args.Add("schema_version", schema_version);
            var queryString = args.ToString();
            if (queryString?.Length > 0) queryString = "?" + queryString;

            return await SendAsync<ICollection<CredentialDefinition>>(HttpMethod.Get, $"/generic/definitions/credentials{queryString}");
        }

        /// <summary>
        /// Create a credential definition
        /// </summary> 
        public async Task<CredentialDefinition> CreateCredentialDefinitionAsync(CreateCredentialDefinition request)
        {
            return await SendAsync<CredentialDefinition>(HttpMethod.Post, $"/generic/definitions/credentials", request);
        }

        /// <summary>
        /// Get credential definition by id
        /// </summary>
        public async Task<CredentialDefinition> GetCredentialDefinitionAsync(string credential_definition_id)
        {
            return await SendAsync<CredentialDefinition>(HttpMethod.Get, $"/generic/definitions/credentials/{credential_definition_id}");
        }

        /// <summary>
        /// Retrieve schemas that the current agent created
        /// </summary> 
        public async Task<ICollection<CredentialSchema>> GetSchemasAsync(
            string? schema_id = null,
            string? schema_issuer_did = null,
            string? schema_name = null,
            string? schema_version = null)
        {
            // construct querystring 
            var args = HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrWhiteSpace(schema_id)) args.Add("schema_id", schema_id);
            if (!string.IsNullOrWhiteSpace(schema_issuer_did)) args.Add("schema_issuer_did", schema_issuer_did);
            if (!string.IsNullOrWhiteSpace(schema_name)) args.Add("schema_name", schema_name);
            if (!string.IsNullOrWhiteSpace(schema_version)) args.Add("schema_version", schema_version);
            var queryString = args.ToString();
            if (queryString?.Length > 0) queryString = "?" + queryString;

            return await SendAsync<ICollection<CredentialSchema>>(HttpMethod.Get, $"/generic/definitions/schemas{queryString}");
        }

        /// <summary>
        /// Create a new schema
        /// </summary> 
        public async Task<CredentialSchema> CreateSchemaAsync(CreateSchema request)
        {
            return await SendAsync<CredentialSchema>(HttpMethod.Post, $"/generic/definitions/schemas", request);
        }

        /// <summary>
        /// Retrieve schema by id
        /// </summary> 
        public async Task<CredentialSchema> GetSchemaAsync(string schema_id)
        {
            return await SendAsync<CredentialSchema>(HttpMethod.Get, $"/generic/definitions/schemas/{schema_id}");
        }

        #endregion Definitions

        #region Webhooks

        /// <summary>
        /// Returns all webhooks per wallet
        /// This implicitly extracts the wallet ID and return only items belonging to the wallet.
        /// </summary> 
        /// TODO: confirm return type
        public async Task<object> GetWebhooksForWalletAsync()
        {
            return await SendAsync<object>(HttpMethod.Get, $"/webhooks");
        }

        /// <summary>
        /// Returns the webhooks per wallet per topic
        /// This implicitly extracts the wallet ID and return only items belonging to the wallet.
        /// </summary> 
        /// <param name="topic">Available values : basic-messages, connections, proofs, credentials, endorsements</param>
        /// TODO: confirm return type
        public async Task<object> GetWebhooksForWalletByTopicAsync(string topic)
        {
            return await SendAsync<object>(HttpMethod.Get, $"/webhooks/{topic}");
        }
        #endregion Webhooks
    }
}
