using System.ComponentModel.DataAnnotations;

namespace Hero.Server.Messages.Requests
{
    public class GetCharacterRequest
    {
        [Required]
        public Guid Id { get; set; }
    }
}
