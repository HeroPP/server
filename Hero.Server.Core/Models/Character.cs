namespace Hero.Server.Core.Models
{
    public class Character
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid GroupId { get; set; }
        public List<NodeTree> NodeTrees { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Race Race { get; set; }

        public void Update(Character character)
        {
            this.Name = character.Name;
            this.Description = character.Description;
            this.NodeTrees = character.NodeTrees;
        }
    }
}
