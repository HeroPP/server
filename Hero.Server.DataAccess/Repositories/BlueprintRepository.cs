﻿using Hero.Server.Core.Logging;
using Hero.Server.Core.Models;
using Hero.Server.Core.Repositories;
using Hero.Server.DataAccess.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hero.Server.DataAccess.Repositories
{
    public class BlueprintRepository : IBlueprintRepository
    {
        private readonly HeroDbContext context;
        private readonly IGroupContext group;
        private readonly ILogger<BlueprintRepository> logger;

        public BlueprintRepository(HeroDbContext context, IGroupContext group, ILogger<BlueprintRepository> logger)
        {
            this.context = context;
            this.group = group;
            this.logger = logger;
        }

        public async Task CreateBlueprintAsync(Blueprint blueprint, CancellationToken cancellationToken = default)
        {
            try
            {
                blueprint.GroupId = this.group.Id;
                await this.context.Blueprints.AddAsync(blueprint, cancellationToken);
                await this.context.SaveChangesAsync(cancellationToken);
                this.logger.LogBlueprintCreated(blueprint.Id);
            }
            catch (Exception ex)
            {
                this.logger.LogBlueprintCreateFailed(blueprint.Id, ex);
                throw;
            }
        }

        public async Task DeleteBlueprintAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                Blueprint? existing = await this.context.Blueprints.FindAsync(new object[] { id }, cancellationToken);

                if (null == existing)
                {
                    this.logger.LogSkilltreeDoesNotExist(id);
                    return;
                }

                this.context.Blueprints.Remove(existing);
                await this.context.SaveChangesAsync(cancellationToken);
                this.logger.LogBlueprintDeleted(id);
            }
            catch (Exception ex)
            {
                this.logger.LogBlueprintDeleteFailed(id, ex);
                throw;
            }
        }

        public Task<List<Blueprint>> GetAllBlueprintsAsync(CancellationToken cancellationToken = default)
        {
            return this.context.Blueprints.ToListAsync(cancellationToken);
        }

        public async Task<Blueprint?> GetBlueprintByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await this.context.Blueprints.Include(print => print.Nodes).SingleOrDefaultAsync(print => print.Id == id, cancellationToken);
        }

        public async Task<Blueprint?> LoadBlueprintByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            Blueprint? blueprint = await this.context.Blueprints.Include(print => print.Nodes).SingleOrDefaultAsync(print => print.Id == id, cancellationToken);

            if (null == blueprint)
            {
                throw new Exception($"The blueprint (id: {id}) you're trying to load does not exist.");
            }

            // Replacing all ids with new Ids so blueprint nodes will become new, unknown skilltree nodes when saved.
            foreach (Node node in blueprint.Nodes)
            {
                Guid newId = Guid.NewGuid();
                Guid oldId = node.Id;

                foreach (Guid precessorId in node.Precessors)
                {
                    Node precessor = blueprint.Nodes.Single(node => node.Id == precessorId);
                    precessor.Successors.Remove(oldId);
                    precessor.Successors.Add(newId);
                }
                foreach (Guid successorId in node.Successors)
                {
                    Node successor = blueprint.Nodes.Single(node => node.Id == successorId);
                    successor.Precessors.Remove(oldId);
                    successor.Precessors.Add(newId);
                }

                node.Id = newId;
            }

            return blueprint;
        }

        public async Task UpdateBlueprintAsync(Guid id, Blueprint updatedBlueprint, CancellationToken cancellationToken = default)
        {
            try
            {
                Blueprint? existing = await this.context.Blueprints.Include(print => print.Nodes).SingleOrDefaultAsync(tree => tree.Id == id, cancellationToken);

                if (null == existing)
                {
                    throw new Exception($"The blueprint (id: {id}) you're trying to update does not exist.");
                }

                existing.Name = updatedBlueprint.Name;

                foreach (Node existingNode in existing.Nodes.Where(node => updatedBlueprint.Nodes.Select(x => x.Id).Contains(node.Id)))
                {
                    Node updatedNode = updatedBlueprint.Nodes.Single(node => existingNode.Id == node.Id);
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

                existing.Nodes.RemoveAll(node => !updatedBlueprint.Nodes.Select(x => x.Id).Contains(node.Id));
                existing.Nodes.AddRange(updatedBlueprint.Nodes.Where(node => !existing.Nodes.Select(x => x.Id).Contains(node.Id)));

                await this.context.SaveChangesAsync(cancellationToken);
                this.logger.LogBlueprintUpdated(id);
            }
            catch (Exception ex)
            {
                this.logger.LogBlueprintUpdateFailed(id, ex);
                throw;
            }
        }
    }
}