using Hero.Server.Core.Models;

namespace Hero.Server.Core.Extensions
{
    public static class CharacterExtensions
    {
        public static List<double> GetActualAttributeValue(this Character character) 
        {
            List<Skill> unlockedSkills = character.GetUnlockedSkills();

            //TODO Return all Attributes Values. Discuss format and place.
            return character.HealthPoints + unlockedSkills.Sum(skill => skill.HealthPointsBoost);
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
