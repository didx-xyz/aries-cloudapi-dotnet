using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AriesCloudAPI.WebClient.UnitTests
{
	internal class MockHttpMessageHandler : HttpMessageHandler
	{
		public HttpRequestMessage LastRequest;

		private readonly IList<HttpResponseMessage> _responsesToReturn;

		public int CallCount { get; private set; }

		public MockHttpMessageHandler(HttpResponseMessage responseToReturn)
		{
			_responsesToReturn = new List<HttpResponseMessage> { responseToReturn };
		}

		public MockHttpMessageHandler(IList<HttpResponseMessage> responsesToReturn)
		{
			_responsesToReturn = responsesToReturn;
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			LastRequest = request;
			CallCount++;
			if (_responsesToReturn.Count <= CallCount - 1)
			{
				return Task.FromResult(_responsesToReturn.Last());
			}
			return Task.FromResult(_responsesToReturn[CallCount - 1]);
		}

		public async Task<T> GetDeserializeRequestContent<T>()
		{
			var respContent = await LastRequest.Content.ReadAsStringAsync();

			var result = JsonSerializer.Deserialize(
				respContent, typeof(T),
				new JsonSerializerOptions()
				{
					IgnoreNullValues = true,
					PropertyNameCaseInsensitive = true,
					Encoder = JavaScriptEncoder.Default
				});

			return (T)result;
		}
	}
}