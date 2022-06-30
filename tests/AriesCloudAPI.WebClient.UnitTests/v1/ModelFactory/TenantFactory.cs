using AriesCloudAPI.WebClient.Commands;
using AriesCloudAPI.WebClient.Models;
using System; 

namespace AriesCloudAPI.WebClient.UnitTests.v1.ModelFactory
{
	public class CreateTenantCommandFactory
	{
		public static Tenant ValidInstance()
		{
			return new CreateTenantCommand
			{
	 Name = "Tenant1", 
			};
		}
	}
}
