using Hero.Server.Core.Logging;
using Hero.Server.Core.Models;
using Hero.Server.Core.Repositories;
using Hero.Server.DataAccess.Database;

using Microsoft.Extensions.Logging;

namespace Hero.Server.DataAccess.Repositories
{
    public class NodeTreeRepository : INodeTreeRepository
    {
        private readonly HeroDbContext context;
        private readonly ILogger<NodeTreeRepository> logger;

        public NodeTreeRepository(HeroDbContext context, ILogger<NodeTreeRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task CreateNodeTreeAsync(NodeTree nodeTree, CancellationToken cancellationToken = default)
        {
            try
            {
                await this.context.NodeTrees.AddAsync(nodeTree, cancellationToken);
                await this.context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
        }

        public async Task DeleteNodeTreeAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                NodeTree? existing = await this.context.NodeTrees.FindAsync(new object[] { id }, cancellationToken);

                if (null == existing)
                {
                    this.logger.LogNodeTreeDoesNotExist(id);
                    return;
                }

                this.context.NodeTrees.Remove(existing);
                await this.context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
        }

        public async Task UpdateNodeTreeAsync(Guid id, NodeTree updatedNodeTree, CancellationToken cancellationToken = default)
        {
            try
            {
                NodeTree? existing = await this.context.NodeTrees.FindAsync(new object[] { id }, cancellationToken);

                if (null == existing)
                {
                    throw new Exception($"The nodeTree (id: {id}) you're trying to update does not exist.");
                }

                existing.Update(updatedNodeTree);

                this.context.NodeTrees.Update(existing);
                await this.context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
        }
    }
}
