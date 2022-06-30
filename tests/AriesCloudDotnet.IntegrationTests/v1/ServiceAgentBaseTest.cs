using AriesCloudDotnet.Clients;
using AriesCloudDotnet.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AriesCloudDotnet.IntegrationTests.v1
{
    public abstract class ServiceAgentBaseTest
	{
		public AriesCloudClientFactory Factory;

		public ServiceAgentBaseTest()
		{
			Factory = new AriesCloudClientFactory(new HttpClient(), Options.Value);
		}

		protected static JsonSerializerOptions CustomJsonSerializerOptions
		{
			get
			{
				var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
				options.Converters.Add(new JsonStringEnumConverter());
				return options;
			}
		}

		public static IOptions<AriesCloudAPIOptions> Options
		{
			get
			{
				var options = new Mock<IOptions<AriesCloudAPIOptions>>();
				options.SetupGet(s => s.Value).Returns(new AriesCloudAPIOptions()
				{
					BaseUri = "http://localhost:8000", 
					APIKey = "UH12s2yEUZihaqEwaDUcat7N7UNLw8Xs"
				});

				return options.Object;
			}
		} 
	}
}