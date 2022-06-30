using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AriesCloudAPI.WebClient.Exceptions;

namespace AriesCloudAPI.WebClient.Extensions
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
