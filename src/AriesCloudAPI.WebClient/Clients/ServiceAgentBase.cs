using AriesCloudAPI.Core.Options;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using AriesCloudAPI.WebClient.Exceptions;
using AriesCloudAPI.WebClient.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text; 
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace AriesCloudAPI.WebClient
{
    public abstract class ServiceAgentBase
	{
		protected readonly HttpClient _client; 
		protected readonly AriesCloudAPIOptions _options; 
		private readonly JsonSerializerSettings _jsonSerializerSettings;
		private readonly JsonSerializerSettings _jsonDeserializerSettings;

		public ServiceAgentBase(
			HttpClient client,
			AriesCloudAPIOptions options, 
			JsonSerializerSettings jsonSerializerSettings = null,
			JsonSerializerSettings jsonDeserializerSettings = null
		)
		{
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_options = options;

			// default serialization settings 
			_jsonSerializerSettings = jsonSerializerSettings;

			// default deserialization settings 
			_jsonDeserializerSettings = jsonDeserializerSettings ?? new JsonSerializerSettings
			{
				// ignore any derserialization errors due to validation attributes on nswag
				Error = (object sender, ErrorEventArgs errorArgs) => { errorArgs.ErrorContext.Handled = true; },
				// ignore case when deserializing 
				//ContractResolver = new TitleCaseContractResolver() 
			};

			//if (string.IsNullOrEmpty(endpointName))
			//{
			//	throw new ArgumentNullException(endpointName);
			//}
 
			if (string.IsNullOrWhiteSpace(_options?.BaseUri))
			{
				throw new InvalidOperationException($"{nameof(_options.BaseUri)} cannot be null, empty or whitespace.");
			}

			_client.BaseAddress = new Uri(_options.BaseUri);
		}

		protected async Task SendAsync(
			HttpMethod method,
			string requestUri,
			object requestContent = null,
			//JsonSerializerOptions jsonSerializerSettings = null,
			//JsonSerializerOptions jsonDeserializerSettings = null,
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
			//JsonSerializerOptions jsonSerializerSettings = null,
			//JsonSerializerOptions jsonDeserializerSettings = null,
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
			//JsonSerializerOptions jsonSerializerSettings,
			//JsonSerializerOptions jsonDeserializerSettings,
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

					return Newtonsoft.Json.JsonConvert.DeserializeObject<TResult>(cachedResponseJson, jsonDeserializerSettings ?? _jsonDeserializerSettings);
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
                    ? Newtonsoft.Json.JsonConvert.DeserializeObject<TResult>(responseJson, jsonDeserializerSettings ?? _jsonDeserializerSettings)
                    : default;
            }
			catch (Exception exception)
			{
				var message = $"An error occured while calling an operation on service agent [{GetType()}], message: {exception.Message}";
				throw new ServiceAgentException(message, exception);
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
			if (httpRequestHeaders == null) return;

            foreach (var httpRequestHeader in httpRequestHeaders)
            { 
				request.Headers.Add(httpRequestHeader.Key, httpRequestHeader.Value);
			} 
		}

		public class TitleCaseContractResolver : DefaultContractResolver
		{
			protected override string ResolvePropertyName(string propertyName)
			{
				//Change the incoming property name into Title case
				var name = string.Concat(propertyName[0].ToString().ToUpper(), propertyName.Substring(1).ToLower());
				return base.ResolvePropertyName(name);
			}
		}

		public class LowerCaseContractResolver : DefaultContractResolver
		{
			protected override string ResolvePropertyName(string propertyName)
			{
				//Change the incoming property name into Lower case
				return base.ResolvePropertyName(propertyName.ToLower());
			}
		}

		public class UpperCaseContractResolver : DefaultContractResolver
		{
			protected override string ResolvePropertyName(string propertyName)
			{
				//Change the incoming property name into Upper case
				return base.ResolvePropertyName(propertyName.ToUpper());
			}
		}
	}
}