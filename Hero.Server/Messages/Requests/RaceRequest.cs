using System.ComponentModel.DataAnnotations;

namespace Hero.Server.Messages.Requests
{
    public class RaceRequest
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<AttributeRaceValueRequest> AttributeRaces { get; set; }

    }
}
