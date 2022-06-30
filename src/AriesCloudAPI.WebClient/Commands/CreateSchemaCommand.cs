namespace AriesCloudAPI.WebClient.Commands
{
    public class CreateSchemaCommand
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string[] Attributes { get; set; }
    }
}
