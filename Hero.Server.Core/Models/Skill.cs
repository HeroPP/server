namespace Hero.Server.Core.Models
{
    public class Skill
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public string? AbilityName { get; set; }
        public string? IconUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<AttributeSkill> AttributeSkills { get; set; }
        public Ability? Ability { get; set; }

        public void Update(Skill skill)
        {
            this.AbilityName= skill.AbilityName;
            this.IconUrl= skill.IconUrl;
            this.Name= skill.Name;
            this.Description= skill.Description;    
            this.AttributeSkills= skill.AttributeSkills;
            this.Ability = skill.Ability;
        }
    }
}
