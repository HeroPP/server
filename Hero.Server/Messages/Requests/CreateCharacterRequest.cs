using Hero.Server.Messages.Responses;
using System.ComponentModel.DataAnnotations;

namespace Hero.Server.Messages.Requests
{
    public class CreateCharacterRequest
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid RaceId { get; set; }
    }
}
