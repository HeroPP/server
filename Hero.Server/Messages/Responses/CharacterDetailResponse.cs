using System.Text.Json.Serialization;

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
        public Guid RaceId { get; set; }
        public RaceResponse Race { get; set; }

        // The full trees are only needed to generate the attribute values, but should not be send over to the client to reduce overhead.
        [JsonIgnore]
        public List<SkilltreeResponse> FullSkilltrees { get; set; } = new();

        public List<SkilltreeOverviewResponse> Skilltrees { get; set; }

        public List<AttributeValueResponse> Attributes => this.GroupAttributes();
     

        private List<AttributeValueResponse> GroupAttributes()
        {
            IEnumerable<AttributeValueResponse> skilltreeAttributes = 
                this.FullSkilltrees.Where(s => s.IsActiveTree).SelectMany(tree => tree.Nodes.Where(node => node.IsUnlocked).SelectMany(node => node.Skill.Attributes)).ToList();

            List<AttributeValueResponse> attributeValueResponses = this.Race.Attributes
                .Concat(skilltreeAttributes)
                .GroupBy(attribute => attribute.AttributeId)
                .Select(group => new AttributeValueResponse()
                {
                    AttributeId = group.Key,
                    Value = group.Sum(a => a.Value),
                    Attribute = group.First().Attribute,
                })
                .ToList();
            return attributeValueResponses; 
        }
    }
}
