using System.ComponentModel.DataAnnotations;

namespace Hero.Server.Messages.Requests
{
    public class CreateRaceRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public List<AttributeValueRequest> Attributes { get; set; }

    }
}
