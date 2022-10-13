namespace Hero.Server.Core.Configuration
{
    public class MappingOptions
    {
        public Dictionary<string, Value> Groups { get; set; }
    }

    public struct Value
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
