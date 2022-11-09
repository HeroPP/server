using AutoMapper;
using Hero.Server.Core.Models;

namespace Hero.Server.Messages.Responses
{
    public class SkillResponse
    {
        public Guid Id { get; set; }
        public Guid? AbilityId { get; set; }
        public string? IconUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AbilityResponse? Ability { get; set; }
        public List<AttributeValueResponse>? Attributes { get; set; }
        
        public SkillResponse(Skill skill, IMapper mapper)
        {
            Id = skill.Id;
            Name = skill.Name;
            Description = skill.Description;
            IconUrl = skill.IconUrl;
            AbilityName = skill.AbilityName;
            Ability = mapper.Map<AbilityResponse>(skill.Ability);
            Attributes = skill.AttributeSkills.Select(ats => (new AttributeValueResponse()
            {
                Id = ats.Attribute.Id,
                Name = ats.Attribute.Name,
                IconUrl = ats.Attribute.IconUrl,
                Description = ats.Attribute.Description,
                StepSize = ats.Attribute.StepSize,
                MinValue = ats.Attribute.MinValue,
                MaxValue = ats.Attribute.MaxValue,
                Value = ats.Value,
            })).ToList();
        }
    }
}