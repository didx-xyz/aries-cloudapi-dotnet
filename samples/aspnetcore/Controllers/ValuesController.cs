using AriesCloudDotnet.Clients;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace aspnetcore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private AriesCloudClientFactory _clientFactory;

        public ValuesController(AriesCloudClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
         
        [HttpGet]
        public async System.Threading.Tasks.Task<IEnumerable<string>> GetAsync()
        {
            // create client
            var client = _clientFactory.CreateTenantClient("TENANT_ID");

            // example usage
            var tenants = await client.GetCredentialsAsync();

            return tenants?.Select(x => x.ToString());
        } 
    }
}
