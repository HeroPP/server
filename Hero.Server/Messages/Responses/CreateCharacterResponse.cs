namespace Hero.Server.Messages.Responses
{
    public class CreateCharacterResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int HealthPoints { get; set; }
        public int LightPoints { get; set; }
        public double MovementSpeed { get; set; }
        public double Resistance { get; set; }
        public double OpticalRange { get; set; }
        public double Parry { get; set; }
        public double Dodge { get; set; }
    }
}
