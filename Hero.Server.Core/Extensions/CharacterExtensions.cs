using Hero.Server.Core.Models;

namespace Hero.Server.Core.Extensions
{
    public static class CharacterExtensions
    {
        public static int GetActualHealthPoints(this Character character)
        {
            List<Skill> unlockedSkills = character.GetUnlockedSkills();

            return character.HealthPoints + unlockedSkills.Sum(skill => skill.HealthPointsBoost);
        }

        public static int GetActualLightPoints(this Character character)
        {
            List<Skill> unlockedSkills = character.GetUnlockedSkills();

            return character.LightPoints + unlockedSkills.Sum(skill => skill.LightPointsBoost);
        }

        public static double GetActualMovementSpeed(this Character character)
        {
            List<Skill> unlockedSkills = character.GetUnlockedSkills();

            return character.MovementSpeed + unlockedSkills.Sum(skill => skill.MovementSpeedBoost);
        }

        public static double GetActualResistance(this Character character)
        {
            List<Skill> unlockedSkills = character.GetUnlockedSkills();

            return character.Resistance + unlockedSkills.Sum(skill => skill.ResistanceBoost);
        }
        public static double GetActualOpticalRange(this Character character)
        {
            List<Skill> unlockedSkills = character.GetUnlockedSkills();

            return character.OpticalRange + unlockedSkills.Sum(skill => skill.OpticalRangeBoost);
        }


        public static double GetTotalDamageBoost(this Character character)
        {
            List<Skill> unlockedSkills = character.GetUnlockedSkills();

            return unlockedSkills.Sum(skill => skill.DamageBoost);
        }

        public static double GetTotalMeleeDamageBoost(this Character character)
        {
            List<Skill> unlockedSkills = character.GetUnlockedSkills();

            return character.GetTotalDamageBoost() + unlockedSkills.Sum(skill => skill.MeleeDamageBoost);
        }

        public static double GetTotalRangeDamageBoost(this Character character)
        {
            List<Skill> unlockedSkills = character.GetUnlockedSkills();

            return character.GetTotalDamageBoost() + unlockedSkills.Sum(skill => skill.RangeDamageBoost);
        }

        public static double GetTotalLightDamageBoost(this Character character)
        {
            List<Skill> unlockedSkills = character.GetUnlockedSkills();

            return character.GetTotalDamageBoost() + unlockedSkills.Sum(skill => skill.LightDamageBoost);
        }

        public static double GetActualParry(this Character character)
        {
            List<Skill> unlockedSkills = character.GetUnlockedSkills();

            return character.Parry + unlockedSkills.Sum(skill => skill.ParryBoost);
        }

        public static double GetActualDodge(this Character character)
        {
            List<Skill> unlockedSkills = character.GetUnlockedSkills();

            return character.Dodge + unlockedSkills.Sum(skill => skill.DodgeBoost);
        }


        public static List<Skill> GetUnlockedSkills(this Character character)
        {
            return character.NodeTrees
                .Where(tree => tree.IsActiveTree)
                .SelectMany(tree => tree.GetAllUnlockedSkills())
                .ToList();
        }
    }
}
