using Hero.Server.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Hero.Server.Messages.Requests
{
    public class CreateNodeTreeRequest
    {
        [Required]
        public Guid? CharacterId { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsActiveTree { get; set; }
        public int Points { get; set; }
        public List<Node> AllNodes { get; set; }


    }
}
