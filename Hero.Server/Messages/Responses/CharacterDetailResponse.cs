using Hero.Server.Core.Extensions;
using Hero.Server.Core.Models;

namespace Hero.Server.Messages.Responses
{
    public class CharacterDetailResponse
    {
        public CharacterDetailResponse(Character character)
        {
            this.Id = character.Id;
            this.Name = character.Name;
            this.Description = character.Description;
            this.HealthPoints = character.GetActualHealthPoints();
            this.LightPoints = character.GetActualLightPoints();
            this.MovementSpeed = character.GetActualMovementSpeed();
            this.Resistance = character.GetActualResistance();
            this.OpticalRange = character.GetActualOpticalRange();
            this.MeleeDamageBuff = character.GetTotalMeleeDamageBoost();
            this.RangeDamageBuff = character.GetTotalRangeDamageBoost();
            this.LightDamageBuff = character.GetTotalLightDamageBoost();
            this.DamageBuff = character.GetTotalDamageBoost();
            this.Parry = character.GetActualParry();
            this.Dodge = character.GetActualDodge();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int HealthPoints { get; set; }
        public int LightPoints { get; set; }
        public double MovementSpeed { get; set; }
        public double Resistance { get; set; }
        public double OpticalRange { get; set; }
        public double MeleeDamageBuff { get; set; }
        public double RangeDamageBuff { get; set; }
        public double LightDamageBuff { get; set; }
        public double DamageBuff { get; set; }
        public double Parry { get; set; }
        public double Dodge { get; set; }
    }
}
