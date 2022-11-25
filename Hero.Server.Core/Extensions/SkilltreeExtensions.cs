using Hero.Server.Core.Models;

namespace Hero.Server.Core.Extensions
{
    public static class SkilltreeExtensions
    {
        public static List<Skill> GetAllUnlockedSkills(this Skilltree tree)
        {
            return tree.Nodes.Where(node => node.IsUnlocked).Select(node => node.Skill).ToList();
        }

        public static bool IsNodeUnlockable(this Skilltree tree, Guid nodeId)
        {
            Node nodeToCheck = tree.Nodes.Single(node => nodeId == node.Id);

            return nodeToCheck.IsEasyReachable
                ? tree.Nodes.Where(node => nodeToCheck.Precessors.Contains(node.Id)).Any(node => node.IsUnlocked)
                : tree.Nodes.Where(node => nodeToCheck.Precessors.Contains(node.Id)).All(node => node.IsUnlocked);
        }
    }
}
