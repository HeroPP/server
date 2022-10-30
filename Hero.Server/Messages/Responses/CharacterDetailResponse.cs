using AutoMapper;
using Hero.Server.Core.Extensions;
using Hero.Server.Core.Models;

namespace Hero.Server.Messages.Responses
{
    public class CharacterDetailResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public RaceResponse Race { get; set; }
        public List<AttributeValueResponse> Attributes { get; set; }
        public List<NodeTreeResponse> NodeTreeResponses { get; set; }

        public CharacterDetailResponse(Character character, IMapper mapper)
        {
            Id = character.Id;
            Name = character.Name;
            Description = character.Description;
            Race = new RaceResponse(character.Race, mapper);
            List<AttributeValueResponse> SkillAttributes = character.NodeTrees.Where(nt => nt.IsActiveTree).SelectMany(nt => nt.GetAllUnlockedSkills()).SelectMany(s => s.AttributeSkills).Select(ats => (new AttributeValueResponse()
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
            Attributes = character.Race.AttributeRaces.Select(ar => (new AttributeValueResponse()
            {
                Id = ar.Attribute.Id,
                Name = ar.Attribute.Name,
                IconUrl = ar.Attribute.IconUrl,
                Description = ar.Attribute.Description,
                StepSize = ar.Attribute.StepSize,
                MinValue = ar.Attribute.MinValue,
                MaxValue = ar.Attribute.MaxValue,
                Value = ar.Value + SkillAttributes.Where(sa => sa.Id == Id).Select(sa => sa.Value).Sum(),
            })).ToList();
            //TODO Nodetree
        }
    }
}
