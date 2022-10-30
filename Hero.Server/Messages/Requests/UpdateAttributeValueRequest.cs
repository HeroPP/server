using System.ComponentModel.DataAnnotations;

namespace Hero.Server.Messages.Requests
{
    public class UpdateAttributeValueRequest
    {
        [Required]
        public Guid AttributeId { get; set; }
        [Required]
        public CreateAttributeRequest CreateAttributeRequest { get; set; }
        [Required]
        public double Value { get; set; }
    }
}
