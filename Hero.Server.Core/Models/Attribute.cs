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

        public List<AttributeRace> AttributeRaces { get; set; }
        public List<AttributeSkill> AttributeSkills { get; set; }
        public void Update(Attribute attribute)
        {
            this.Name = attribute.Name;
            this.IconUrl = attribute.IconUrl;
            this.Description = attribute.Description;
            this.StepSize = attribute.StepSize;
            this.MinValue = attribute.MinValue;
            this.MaxValue = attribute.MaxValue;
            this.AttributeRaces = attribute.AttributeRaces;
            this.AttributeSkills = attribute.AttributeSkills;
        }
    }
}
