using System;
using System.Net;

namespace AriesCloudDotnet.Clients.Base.Exceptions
{
	public class HttpClientException : Exception
	{
		public override string Message => $"HttpStatusCode={StatusCode}, {base.Message}";
        public HttpStatusCode StatusCode { get; }

        public HttpClientException(HttpStatusCode statuscode, string message) : base(message)
        {
            StatusCode = statuscode;
        }
	}
}