using Hero.Server.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hero.Server.Core.Repositories
{
    public interface INodeRepository
    {
        Task CreateNodeAsync(Node node, CancellationToken cancellationToken = default);
        Task UpdateNodeAsync(Guid id, Node updatedNode, CancellationToken cancellationToken = default);
        Task DeleteNodeAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
