using Hero.Server.Core.Models;

namespace Hero.Server.Core.Configuration
{
    public class MappingOptions
    {
        public Dictionary<string, MappingValue> Roles { get; set; }
        public Dictionary<string, MappingValue> Groups { get; set; }
    }

    public struct MappingValue
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
