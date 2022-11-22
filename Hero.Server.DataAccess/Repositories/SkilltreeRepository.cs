﻿using Hero.Server.Core.Exceptions;
using Hero.Server.Core.Logging;
using Hero.Server.Core.Models;
using Hero.Server.Core.Repositories;
using Hero.Server.DataAccess.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hero.Server.DataAccess.Repositories
{
    public class SkilltreeRepository : ISkilltreeRepository
    {
        private readonly HeroDbContext context;
        private readonly IGroupContext group;
        private readonly ILogger<SkilltreeRepository> logger;

        public SkilltreeRepository(HeroDbContext context, IGroupContext group, ILogger<SkilltreeRepository> logger)
        {
            this.context = context;
            this.group = group;
            this.logger = logger;
        }

        public async Task<List<Skilltree>> FilterSkilltrees(Guid? characterId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await this.context.Skilltrees
                    .Include(tree => tree.Nodes)
                    .Where(c => null == characterId || c.CharacterId == characterId)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw new HeroException("An error occured while getting a list of skilltrees.");
            }
        }

        public async Task<Skilltree?> GetSkilltreeByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await this.context.Skilltrees
                    .Include(c => c.Character)
                    .Include(c => c.Nodes).ThenInclude(n => n.Skill).ThenInclude(s => s.Ability)
                    .Include(s => s.Nodes).ThenInclude(n => n.Skill).ThenInclude(s => s.Attributes).ThenInclude(a => a.Attribute)
                    .FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw new HeroException("An error occured while getting w skilltree.");
            }
        }

        public async Task CreateSkilltreeAsync(Skilltree skilltree, CancellationToken cancellationToken = default)
        {
            try
            {
                skilltree.GroupId = this.group.Id;
                await this.context.Skilltrees.AddAsync(skilltree, cancellationToken);
                await this.context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw new HeroException("An error occured while creating skilltree.");
            }
        }

        public async Task DeleteSkilltreeAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                Skilltree? existing = await this.context.Skilltrees.FindAsync(new object[] { id }, cancellationToken);

                if (null == existing)
                {
                    this.logger.LogSkilltreeDoesNotExist(id);
                    throw new ObjectNotFoundException("The skilltree you are looking for could not be found.");
                }

                this.context.Skilltrees.Remove(existing);
                await this.context.SaveChangesAsync(cancellationToken);
            }
            catch (HeroException ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw new HeroException("An error occured while deleting skilltree.");
            }
        }

        public async Task UpdateSkilltreeAsync(Guid id, Skilltree updatedTree, CancellationToken cancellationToken = default)
        {
            try
            {
                Skilltree? existing = await this.context.Skilltrees.Include(tree => tree.Nodes).SingleOrDefaultAsync(tree => tree.Id == id, cancellationToken);

                if (null == existing)
                {
                    throw new ObjectNotFoundException($"The skilltree (id: {id}) you're trying to update does not exist.");
                }

                existing.Name = updatedTree.Name;
                existing.Points = updatedTree.Points;
                existing.CharacterId = updatedTree.CharacterId;
                existing.IsActiveTree = updatedTree.IsActiveTree;
                
                foreach (Node existingNode in existing.Nodes.Where(node => updatedTree.Nodes.Select(x => x.Id).Contains(node.Id)))
                {
                    Node updatedNode = updatedTree.Nodes.Single(node => existingNode.Id == node.Id);
                    existingNode.Successors = updatedNode.Successors;
                    existingNode.Precessors = updatedNode.Precessors;
                    existingNode.Color = updatedNode.Color;
                    existingNode.Cost = updatedNode.Cost;
                    existingNode.XPos = updatedNode.XPos;
                    existingNode.YPos = updatedNode.YPos;
                    existingNode.Importance = updatedNode.Importance;
                    existingNode.IsEasyReachable = updatedNode.IsEasyReachable;
                    existingNode.SkillId = updatedNode.SkillId;
                }

                existing.Nodes.RemoveAll(node => !updatedTree.Nodes.Select(x => x.Id).Contains(node.Id));
                existing.Nodes.AddRange(updatedTree.Nodes.Where(node => !existing.Nodes.Select(x => x.Id).Contains(node.Id)));

                await this.context.SaveChangesAsync(cancellationToken);
            }
            catch (HeroException ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw new HeroException("An error occured while updating skilltree.");
            }
        }
    }
}
