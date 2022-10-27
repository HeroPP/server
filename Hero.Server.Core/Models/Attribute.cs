namespace Hero.Server.Core.Models
{
    public class Attribute
    {
        public Guid Id { get; set; }
        public Guid? GroupId { get; set; }
        public string Name { get; set; }
        public string? IconUrl { get; set; }
        public string? Description { get; set; }
        public double StepSize { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }

        public List<AttributeCharacter> AttributeCharcters { get; set; }

        public void Update(Attribute attribute)
        {

        }
    }
}
