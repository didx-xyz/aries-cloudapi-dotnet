using AriesCloudAPI.WebClient.Models;
using System; 

namespace AriesCloudAPI.WebClient.UnitTests.v1.ModelFactory
{
	public class TenantFactory
	{
		public static Tenant ValidInstance()
		{
			return new Tenant
			{
				 Tenant_id = "Tenant_id",
				 Image_url= "Image_url",
				 Tenant_name= "Tenant_name",
				 AdditionalProperties= null,
				 Created_at = null,
				 Updated_at = null
			};
		}
	}
}
