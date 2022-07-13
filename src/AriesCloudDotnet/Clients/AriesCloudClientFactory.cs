using AriesCloudDotnet.Configuration;
using System.Net.Http;
using System.Collections;
using System.Collections.Generic;

namespace AriesCloudDotnet.Clients
{
    public class AriesCloudClientFactory
    {
        private HttpClient _client;
        private AriesCloudAPIOptions _options;

        public AriesCloudClientFactory(HttpClient client, AriesCloudAPIOptions options)
        {
            _client = client;
            _options = options;
        }

        public AriesCloudClient CreateTenantClient(string tenantId)
        {
            // add authorization header  
            var httpRequestHeaders = new Dictionary<string, string>
            {
                { AriesCloudConstants.Header_ApiKey, _options.APIKey },
                { AriesCloudConstants.Header_TenantId, tenantId }
            };

            return createClient(httpRequestHeaders);
        }

        public AriesCloudClient CreateTenantAdminClient()
        {
            // add authorization headers
            var httpRequestHeaders = new Dictionary<string, string>
            {
                { AriesCloudConstants.Header_ApiKey, _options.APIKey },
                { AriesCloudConstants.Header_TenantId, AriesCloudConstants.TenantId_TenantAdmin }
            };

            return createClient(httpRequestHeaders);
        }

        public AriesCloudClient CreateGoveranceClient()
        {
            // add authorization headers
            var httpRequestHeaders = new Dictionary<string, string>
            {
                { AriesCloudConstants.Header_ApiKey, _options.APIKey },
                { AriesCloudConstants.Header_TenantId, AriesCloudConstants.TenantId_Goverance }
            };

            return createClient(httpRequestHeaders);
        }

        private AriesCloudClient createClient(Dictionary<string, string> httpRequestHeaders)
        {
            return new AriesCloudClient(/*_client, */_options, httpRequestHeaders: httpRequestHeaders);
        }
    }
}
