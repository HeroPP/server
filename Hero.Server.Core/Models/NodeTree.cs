namespace Hero.Server.Core.Models
{
    public class NodeTree
    {
        public Guid Id { get; set; }
        public Guid? CharacterId { get; set; }
        public string Name { get; set; }
        public bool IsActiveTree { get; set; }
        public int Points { get; set; }
        public List<Node> AllNodes { get; set; }

        public void Update(NodeTree nodeTree)
        {
            this.Name = nodeTree.Name;
            this.Points = nodeTree.Points;
            this.IsActiveTree = nodeTree.IsActiveTree;
            this.AllNodes = nodeTree.AllNodes;
        }
    }
}
