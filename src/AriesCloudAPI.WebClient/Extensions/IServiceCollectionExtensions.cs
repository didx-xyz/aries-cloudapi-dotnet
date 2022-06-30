using AriesCloudAPI.WebClient.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Omniplan.ServiceAgent.Extensions
{
    public static class IServiceCollectionExtensions
	{
		public static void AddServiceAgentBase(this IServiceCollection services, IConfiguration config)
		{
			services.AddScoped<TenantServiceAgent>();

			//services.AddOptions();
			//services.AddHttpContextAccessor();
			//services.Configure<ServiceAgentConfiguration>(config.GetSection(nameof(ServiceAgentConfiguration)));
			//services.TryAddSingleton<IAuthenticationMemoryCache, AuthenticationMemoryCache>();
			//services.TryAddTransient<IAuthenticationProvider, OpenIdAuthenticationProvider>();
			//services.AddHttpClient("delegation");
		}
	}
}
