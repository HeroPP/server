namespace Hero.Server.Core.Models
{
    public class Character
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public List<NodeTree> NodeTrees { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int HealthPoints { get; set; }
        public int LightPoints { get; set; }
        public double MovementSpeed { get; set; }
        public double Resistance { get; set; }
        public double OpticalRange { get; set; }
        //public double MeleeDamageBuff { get; set; }
        //public double RangeDamageBuff { get; set; }
        //public double LightDamageBuff { get; set; }
        //public double DamageBuff { get; set; }
        public double Parry { get; set; }
        public double Dodge { get; set; }

        public User? User { get; set; }

        public void Update(Character updated)
        {
            this.Name = updated.Name;
            this.Description = updated.Description;
            this.HealthPoints = updated.HealthPoints;
            this.LightPoints = updated.LightPoints;
            this.MovementSpeed = updated.MovementSpeed;
            this.Resistance = updated.Resistance;
            this.OpticalRange = updated.OpticalRange;
            this.Parry = updated.Parry;
            this.Dodge = updated.Dodge;
        }
    }
}
