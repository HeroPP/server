using AutoMapper;
using Hero.Server.Core.Models;

namespace Hero.Server.Messages.Responses
{
    public class RaceResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<AttributeValueResponse> Attributes { get; set; }
        public RaceResponse(Race race, IMapper mapper)
        {
            Id = race.Id;
            Name = race.Name;
            Description = race.Description;
            Attributes = race.AttributeRaces.Select(ar => (new AttributeValueResponse()
            {
                Id = ar.Attribute.Id,
                Name = ar.Attribute.Name,
                IconUrl = ar.Attribute.IconUrl,
                Description = ar.Attribute.Description,
                StepSize = ar.Attribute.StepSize,
                MinValue = ar.Attribute.MinValue,
                MaxValue = ar.Attribute.MaxValue,
                Value = ar.Value,
            })).ToList();
        }
    }
}