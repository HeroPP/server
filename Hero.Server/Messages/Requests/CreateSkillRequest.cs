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
        public string? AbilityName { get; set; }
        public CreateAbilityRequest CreateAbilityRequest { get; set; }
        public List<UpdateAttributeValueRequest>? UpdateAttributeValueRequests { get; set; }
    }
}
