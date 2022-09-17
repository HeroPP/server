using Hero.Server.Core.Models;

namespace Hero.Server.Core.Extensions
{
    public static class CharacterExtensions
    {
        public static int GetActualHealthPoints(this Character character)
        {
            List<Skill> unlockedSkills = character.NodeTrees
                .Where(tree => tree.IsActiveTree)
                .SelectMany(tree => tree.GetAllUnlockedSkills())
                .ToList();

            return character.HealthPoints + unlockedSkills.Sum(skill => skill.HealthPointsBoost);
        }
    }
}
