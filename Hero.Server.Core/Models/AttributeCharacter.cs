namespace Hero.Server.Core.Models
{
    public class AttributeCharacter
    {
        public Guid AttributeId { get; set; }
        public Guid CharacterId { get; set; }
        public Attribute Attribute { get; set; }
        public Character Character { get; set; }


    }
}
