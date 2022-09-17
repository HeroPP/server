using Hero.Server.Core.Models;

namespace Hero.Server.Core.Repositories
{
    public interface INodeTreeRepository
    {
        Task CreateNodeTreeAsync(NodeTree nodeTree, CancellationToken cancellationToken = default);
        Task UpdateNodeTreeAsync(Guid id, NodeTree updatedNodeTree, CancellationToken cancellationToken = default);
        Task DeleteNodeTreeAsync(Guid id, CancellationToken cancellationToken = default);
    }
}