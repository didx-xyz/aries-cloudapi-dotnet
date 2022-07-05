using AriesCloudDotnet.Clients.Base.Exceptions;
using AriesCloudDotnet.Clients.Base.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Text;

namespace AriesCloudDotnet.Clients
{
    /// <summary>
    /// Base class for http requests
    /// </summary>
    public abstract class ServiceBase
	{
		protected readonly HttpClient _client; 
		private readonly JsonSerializerSettings _jsonSerializerSettings;
		private readonly JsonSerializerSettings _jsonDeserializerSettings;
		private Dictionary<string, string> _httpRequestHeaders;

		public ServiceBase(
			//HttpClient client, 
			string baseUri,
			JsonSerializerSettings jsonSerializerSettings = null,
			JsonSerializerSettings jsonDeserializerSettings = null,
			Dictionary<string, string> httpRequestHeaders = null
		)
		{
			//_client = client ?? throw new ArgumentNullException(nameof(client)); 
			_client = new HttpClient();

			// default serialization settings 
			_jsonSerializerSettings = jsonSerializerSettings;

			// default deserialization settings 
			_jsonDeserializerSettings = jsonDeserializerSettings ?? new JsonSerializerSettings
			{
				// ignore any derserialization errors due to validation attributes on nswag
				Error = (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs errorArgs) => { errorArgs.ErrorContext.Handled = true; }
			}; 
 
			if (string.IsNullOrWhiteSpace(baseUri))
			{
				throw new InvalidOperationException($"{nameof(baseUri)} cannot be null, empty or whitespace.");
			}

			_client.BaseAddress = new Uri(baseUri);

			_httpRequestHeaders = httpRequestHeaders;
		}

		protected async Task SendAsync(
			HttpMethod method,
			string requestUri,
			object requestContent = null, 
			JsonSerializerSettings jsonSerializerSettings = null,
			JsonSerializerSettings jsonDeserializerSettings = null,
			IMemoryCache cache = null,
			bool throwOnFailure = true,
			Dictionary<string, string> httpRequestHeaders = null
		)
		{
			await SendAsyncInternal<int>(
				method,
				requestUri,
				requestContent,
				jsonSerializerSettings,
				jsonDeserializerSettings,
				cache,
				false,
				throwOnFailure,
				httpRequestHeaders
			);
		}

		protected async Task<TResult> SendAsync<TResult>(
			HttpMethod method,
			string requestUri,
			object requestContent = null, 
			JsonSerializerSettings jsonSerializerSettings = null,
			JsonSerializerSettings jsonDeserializerSettings = null,
			IMemoryCache cache = null,
			bool throwOnFailure = true,
			Dictionary<string, string> httpRequestHeaders = null
		)
		{
			return await SendAsyncInternal<TResult>(
				method,
				requestUri,
				requestContent,
				jsonSerializerSettings,
				jsonDeserializerSettings,
				cache,
				true,
				throwOnFailure, 
				httpRequestHeaders
			);
		}

		private async Task<TResult> SendAsyncInternal<TResult>(
			HttpMethod method,
			string requestUri,
			object requestContent, 
			JsonSerializerSettings jsonSerializerSettings ,
			JsonSerializerSettings jsonDeserializerSettings  ,
			IMemoryCache cache,
			bool jsonConvertResponse,
			bool throwOnFailure = true,
			Dictionary<string, string> httpRequestHeaders = null
		)
		{
			try
			{
				var requestJson = (requestContent == null || requestContent.GetType() == typeof(string)
					? (string)requestContent
					: Newtonsoft.Json.JsonConvert.SerializeObject(requestContent, jsonSerializerSettings ?? _jsonSerializerSettings));

				var hashCode = new { requestUri, requestJson }.GetHashCode();

				if (cache != null && cache.TryGetValue(hashCode, out HttpResponseMessage cachedResponse))
				{
					var cachedResponseJson = await cachedResponse.Content.ReadAsStringAsync();

					return JsonConvert.DeserializeObject<TResult>(cachedResponseJson, jsonDeserializerSettings ?? _jsonDeserializerSettings);
				}

				var response = await _client.SendAsync(GenerateHttpRequestMessage(method, requestUri, requestJson, httpRequestHeaders));
				if (throwOnFailure)
				{
					await response.EnsureSuccessStatusCodeAsync();
				}

				var responseJson = response.Content != null ? await response.Content.ReadAsStringAsync() : null;

				if (string.IsNullOrWhiteSpace(responseJson))
				{
					return default;
				}

				if (cache != null && method == HttpMethod.Get)
				{
					cache.Set(hashCode, response, TimeSpan.FromMinutes(45));
				}

                return jsonConvertResponse
                    ? JsonConvert.DeserializeObject<TResult>(responseJson, jsonDeserializerSettings ?? _jsonDeserializerSettings)
                    : default;
            }
			catch (Exception exception)
			{
				var message = $"An error occured while calling an operation on service agent [{GetType()}], message: {exception.Message}";
				throw new ServiceException(message, exception);
			}
		}

		//protected async Task<TResult> PostMultiPartFormDataAsync<TResult>(
		//		string requestUri,
		//		Dictionary<string, object> postParameters,
		//		CancellationToken cancellationToken,
		//		JsonSerializerOptions jsonDeserializerSettings = null,
		//		bool throwOnFailure = true) where TResult : new()
		//{
		//	string boundary = "----------omniplan-boundary";

		//	var response = await InternalPostMultiPartFormDataAsync(
		//						   requestUri,
		//						   boundary,
		//						   MultipartFormDataRequestBuilder.CreateMultipartFormDataRequest(postParameters, boundary),
		//						   cancellationToken,
		//						   throwOnFailure);

		//	return JsonSerializer.Deserialize<TResult>(response, jsonDeserializerSettings ?? _jsonDeserializerSettings);
		//}

		//protected async Task<T> PostMultiPartFormDataAsync<T>(
		//		string requestUri,
		//		Dictionary<string, object> postParameters,
		//		CancellationToken cancellationToken,
		//		bool throwOnFailure = true) where T : IConvertible
		//{
		//	string boundary = "----------omniplan-boundary";

		//	var response = await InternalPostMultiPartFormDataAsync(
		//			requestUri,
		//			boundary,
		//			MultipartFormDataRequestBuilder.CreateMultipartFormDataRequest(postParameters, boundary),
		//			cancellationToken,
		//			throwOnFailure);

		//	return (T)Convert.ChangeType(response, typeof(T));
		//}

		//private async Task<string> InternalPostMultiPartFormDataAsync(
		//		string requestUri,
		//		string boundary,
		//		byte[] formData,
		//		CancellationToken cancellationToken,
		//		bool throwOnFailure = true)
		//{
		//	if (formData == null || formData.Length == 0)
		//	{
		//		throw new ArgumentNullException(nameof(formData));
		//	}

		//	try
		//	{
		//		string contentType = "multipart/form-data; boundary=" + boundary;

		//		var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUri);
		//		httpRequest.Content = new StreamContent(new MemoryStream(formData));
		//		httpRequest.Content.Headers.Add("content-type", contentType);

		//		await DecorateHttpRequestWithAuthentication(httpRequest);

		//		var response = await _client.SendAsync(httpRequest, cancellationToken);
		//		if (throwOnFailure)
		//		{
		//			await response.EnsureSuccessStatusCodeAsync();
		//		}

		//		return await response.Content.ReadAsStringAsync();
		//	}
		//	catch (Exception exception)
		//	{
		//		var message = $"An error occured while calling an operation on service agent [{GetType()}]";
		//		throw new ServiceAgentException(message, exception);
		//	}
		//}

		private HttpRequestMessage GenerateHttpRequestMessage(HttpMethod method, string operationPath, string content, Dictionary<string, string> httpRequestHeaders)
		{
			var stringContent = content != null
				? new StringContent(content, Encoding.UTF8, "application/json") : null;

			var request = new HttpRequestMessage
			{
				RequestUri = new Uri(_client.BaseAddress, operationPath),
				Content = stringContent,
				Method = method
			};

			AddHttpRequestHeaders(request, httpRequestHeaders);

			return request;
		}

		private void AddHttpRequestHeaders(HttpRequestMessage request, Dictionary<string, string> httpRequestHeaders)
		{
			var headers = httpRequestHeaders ?? _httpRequestHeaders;
			if (headers == null) return;

			foreach (var header in headers)
            { 
				request.Headers.Add(header.Key, header.Value);
			} 
		} 
	}
}