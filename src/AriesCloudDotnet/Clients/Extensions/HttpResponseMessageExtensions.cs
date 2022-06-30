using AriesCloudDotnet.Clients.Base.Exceptions;
using System.Net.Http;
using System.Threading.Tasks;

namespace AriesCloudDotnet.Clients.Base.Extensions
{
    public static class HttpResponseMessageExtensions
	{
		/// <summary>
		/// Throws an exception if the <see cref="P:System.Net.HttpResponseMessage.IsSuccessStatusCode"/> property for the HTTP repsonse is false
		/// </summary>
		/// <param name="response"></param>
		/// <returns></returns>
		public static async Task EnsureSuccessStatusCodeAsync(this HttpResponseMessage response)
		{
			if (response.IsSuccessStatusCode)
			{
				return;
			}

			var content = await response.Content.ReadAsStringAsync();
			response.Content?.Dispose();

			throw new HttpClientException(response.StatusCode, content);
		}
	}
}
