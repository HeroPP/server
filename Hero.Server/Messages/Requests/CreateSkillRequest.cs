using Hero.Server.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Hero.Server.Messages.Requests
{
    public class CreateSkillRequest
    {
        public Guid? Id { get; set; }
        public string? IconUrl { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? AbilityId { get; set; }
        public List<AttributeValueRequest> Attributes { get; set; }
    }
}
