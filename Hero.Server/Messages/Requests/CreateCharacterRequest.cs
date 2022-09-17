using System.ComponentModel.DataAnnotations;

namespace Hero.Server.Messages.Requests
{
    public class CreateCharacterRequest
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }


        [Required, Range(0, int.MaxValue)]
        public int HealthPoints { get; set; }


        [Required, Range(0, double.MaxValue)]
        public int LightPoints { get; set; }


        [Required, Range(0, double.MaxValue)]
        public double MovementSpeed { get; set; }

        [Required, Range(0, double.MaxValue)]
        public double Resistance { get; set; }

        [Required, Range(0, double.MaxValue)]
        public double OpticalRange { get; set; }

        [Required, Range(0, 100)]
        public double Parry { get; set; }

        [Required, Range(0, 100)]
        public double Dodge { get; set; }
    }
}
