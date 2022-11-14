using AutoMapper;
using Hero.Server.Core.Extensions;
using Hero.Server.Core.Models;

namespace Hero.Server.Messages.Responses
{
    public class CharacterDetailResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? IconUrl { get; set; }
        public int? Age { get; set; }
        public string? Inventory { get; set; }
        public string? Religion { get; set; }
        public string? Relationship { get; set; }
        public string? Notes { get; set; }
        public string? Profession { get; set; }
        public RaceResponse Race { get; set; }
        public List<AttributeValueResponse> Attributes { get; set; }
        public List<SkilltreeResponse> SkilltreeResponses { get; set; }

    }
}
