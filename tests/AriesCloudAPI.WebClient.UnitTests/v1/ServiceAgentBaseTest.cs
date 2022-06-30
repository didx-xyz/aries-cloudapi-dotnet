using AriesCloudAPI.Core.Options;
using AriesCloudAPI.WebClient.Core;
using Microsoft.Extensions.Options;
using Moq; 
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AriesCloudAPI.WebClient.UnitTests.v1
{
    public abstract class ServiceAgentBaseTest
	{
		//protected const string BaseUrl = "http://localhost:8000/api";

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
					BaseUri = "http://localhost:8000"
					//	Endpoints = new List<EndpointConfiguration>()
					//{
					//	new EndpointConfiguration()
					//	{
					//		Name = "CalculationApiService",
					//		Address = "http://localhost/api/v1/"
					//	}
					//}
				});

				return options.Object;
			}
		}

		//public IAuthenticationProvider AuthenticationProvider
		//{
		//	get
		//	{
		//		var authenticationProvider = new Mock<IAuthenticationProvider>();
		//		authenticationProvider.Setup(x => x.GetAccessToken(It.IsAny<string>())).ReturnsAsync(string.Empty);
		//		return authenticationProvider.Object;
		//	}
		//}
	}
}