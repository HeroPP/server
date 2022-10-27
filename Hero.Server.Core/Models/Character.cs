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
        public List<AttributeCharacter> AttributeCharcters { get; set; }

        public void Update(Character updated)
        {
            this.Name = updated.Name;
            this.Description = updated.Description;

        }
    }
}
