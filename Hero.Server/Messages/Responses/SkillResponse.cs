using Hero.Server.Core.Models;

namespace Hero.Server.Messages.Responses
{
    public class SkillResponse
    {
        public Guid Id { get; set; }
        public string? AbilityName { get; set; }
        public string? IconUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int HealthPointsBoost { get; set; }
        public int LightPointsBoost { get; set; }
        public double MovementSpeedBoost { get; set; }
        public double ResistanceBoost { get; set; }
        public double OpticalRangeBoost { get; set; }
        public double MeleeDamageBoost { get; set; }
        public double RangeDamageBoost { get; set; }
        public double LightDamageBoost { get; set; }
        public double DamageBoost { get; set; }
        public double ParryBoost { get; set; }
        public double DodgeBoost { get; set; }
        //?
        public AbilityResponse? Ability { get; set; }
    }
}