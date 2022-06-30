using System; 

namespace AriesCloudAPI.WebClient.Exceptions
{
	[Serializable]
	public class ServiceAgentException : Exception
	{
		public ServiceAgentException()
		{
		}

		public ServiceAgentException(string message) : base(message)
		{
		}

		public ServiceAgentException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
