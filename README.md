# aries-cloudapi-dotnet
This is a .NET SDK for [Aries Cloud APIM](https://github.com/didx-xyz/aries-cloudapi-apim). It facilitates the configuration & communication to the Aries Cloud APIM for .NET consumers.

## Install (incomplete)

### Package Manager Console
```
PM> Install-Package AriesCloudDotnet
```
### .NET CLI Console
```
> dotnet add package AriesCloudDotnet
```
 
## Configuration
Add the following configuration to your appsettings.json:
```
{
  ... 
  "AriesCloudAPI": {
    "BaseUri": "http://localhost:8000",
    "APIKey": "{API_KEY}"
  }
}
```
- `BaseUri` is the url of the Aries Cloud APIM
- `APIKey` is your consumer api-key

## Startup
Add the following to your application startup:

```
        public void ConfigureServices(IServiceCollection services)
        {
            ...
 
            services.AddAriesCloudAPI(Configuration);
        }
```
```
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ...
 
            app.UseAriesCloudAPI();
        }
```

## Usage
To communicate with the Aries Cloud APIM, a client-proxy must first be created via the `AriesCloudClientFactory` class. 

This is provided by the Dependency Injection. 

It provides the following methods to create clients for the respective contexts:

- CreateTenantClient(string tenantId)
_creates a client for the specified tenantId_

- CreateTenantAdminClient()
_creates a client under the context of the 'governance' role as defined in Aries Cloud API._

- CreateGoveranceClient()
_creates a client under the context of the 'tenant-admin' role as defined in Aries Cloud API._

Example:
```
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
            var client = _clientFactory.CreateTenantClient("{TENANT_ID}");

            // example usage
            var tenants = await client.GetCredentialsAsync();

            return tenants?.Select(x => x.ToString());
        } 
    }
```
_for more exmaples, see the `AriesCloudDotnet.IntegrationTests` project in the source code._