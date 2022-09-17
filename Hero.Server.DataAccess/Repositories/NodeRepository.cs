using Hero.Server.Core.Logging;
using Hero.Server.Core.Models;
using Hero.Server.Core.Repositories;
using Hero.Server.DataAccess.Database;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hero.Server.DataAccess.Repositories
{
    internal class NodeRepository : INodeRepository
    {
        private readonly HeroDbContext context;
        private readonly ILogger<NodeRepository> logger;

        public NodeRepository(HeroDbContext context, ILogger<NodeRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }


        public async Task CreateNodeAsync(Node node, CancellationToken cancellationToken = default)
        {
            try
            {
                await this.context.Nodes.AddAsync(node, cancellationToken);
                await this.context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
        }

        public async Task DeleteNodeAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
               Node? existing = await this.context.Nodes.FindAsync(new object[] { id }, cancellationToken);
                if (null == existing)
                {
                    this.logger.LogNodeDoesNotExist(id);
                    return;
                }
                this.context.Nodes.Remove(existing);
                await this.context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
        }


        public async Task UpdateNodeAsync(Guid id,Node updatedNode, CancellationToken cancellationToken = default)
        {
            try
            {
               Node? existing = await this.context.Nodes.FindAsync(new object[] { id }, cancellationToken);

                if (null == existing)
                {
                    throw new Exception($"TheNode (id: {id}) you're trying to update does not exist.");
                }

                existing.Update(updatedNode);

                this.context.Nodes.Update(existing);
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
