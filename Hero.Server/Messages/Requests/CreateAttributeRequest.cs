using System.ComponentModel.DataAnnotations;

namespace Hero.Server.Messages.Requests
{
    public class CreateAttributeRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public Guid Id { get; set; }
        [Required]
        public double MaxValue { get; set; }
        [Required]
        public double MinValue { get; set; }
        [Required]
        public double StepSize { get; set; }

    }
}
