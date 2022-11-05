using Hero.Server.Core.Models;

namespace Hero.Server.Core.Extensions
{
    public static class NodeTreeExtensions
    {
        public static List<Skill> GetAllUnlockedSkills(this Skilltree tree)
        {
            return tree.Nodes.Where(node => node.IsUnlocked).Select(node => node.Skill).ToList();
        }
    }
}
