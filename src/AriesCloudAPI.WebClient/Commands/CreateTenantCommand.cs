namespace AriesCloudAPI.WebClient.Commands
{
    public class CreateTenantCommand
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public TenantRole[] Roles { get; set; }
    }
}
