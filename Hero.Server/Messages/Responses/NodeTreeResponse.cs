using Hero.Server.Core.Models;

namespace Hero.Server.Messages.Responses
{
    public class NodeTreeResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActiveTree { get; set; }
        public int Points { get; set; }
        public List<Node> AllNodes { get; set; }
    }
}
