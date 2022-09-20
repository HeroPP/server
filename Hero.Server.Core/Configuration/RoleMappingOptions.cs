namespace Hero.Server.Core.Configuration
{
    public class RoleMappingOptions
    {
        public Dictionary<string, Role> RoleMapping { get; set; }
    }

    public struct Role
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
