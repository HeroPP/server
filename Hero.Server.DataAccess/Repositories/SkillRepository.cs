﻿using Hero.Server.Core.Logging;
using Hero.Server.Core.Models;
using Hero.Server.Core.Repositories;
using Hero.Server.DataAccess.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace Hero.Server.DataAccess.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly HeroDbContext context;
        private readonly IGroupContext group;
        private readonly IUserRepository userRepository;
        private readonly ILogger<SkillRepository> logger;

        public SkillRepository(HeroDbContext context, IGroupContext group, IUserRepository userRepository, ILogger<SkillRepository> logger)
        {
            this.context = context;
            this.group = group;
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public async Task<Skill?> GetSkillByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await this.context.Skills.Include(s => s.Ability).Include(s => s.AttributeSkills).ThenInclude(ats => ats.Attribute).FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task CreateSkillAsync(Skill skill, CancellationToken cancellationToken = default)
        {
            try
            {
                skill.GroupId = group.Id;
                skill.Id = Guid.NewGuid();
                skill.AttributeSkills.ForEach(ats => ats.SkillId = skill.Id);

                await this.context.Skills.AddAsync(skill, cancellationToken);
                await this.context.SaveChangesAsync(cancellationToken);                
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
        }

        public async Task DeleteSkillAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                Skill? existing = await this.GetSkillByIdAsync(id, cancellationToken);
                if (null == existing)
                {
                    this.logger.LogSkillDoesNotExist(id);
                    return;
                }
                this.context.Skills.Remove(existing);
                await this.context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
        }

        public async Task<IEnumerable<Skill>> GetAllSkillsAsync(CancellationToken cancellationToken = default)
        {
            return await this.context.Skills.Include(s => s.Ability).Include(s => s.AttributeSkills).ThenInclude(ats => ats.Attribute).ToListAsync(cancellationToken);
        }

        public async Task UpdateSkillAsync(Guid id, Skill updatedSkill, CancellationToken cancellationToken = default)
        {
            try
            {
                Skill? existing = await this.GetSkillByIdAsync(id, cancellationToken);

                if (null == existing)
                {
                    throw new Exception($"The Skill (id: {id}) you're trying to update does not exist.");
                }

                existing.Update(updatedSkill);

                foreach (AttributeSkill existingAttributeSkill in existing.AttributeSkills.Where(ats => updatedSkill.AttributeSkills.Select(x => (x.SkillId, x.AttributeId)).Contains((ats.SkillId, ats.AttributeId))))
                {
                    AttributeSkill updatedAttributeSkill = updatedSkill.AttributeSkills.Single(ats => existingAttributeSkill.SkillId == ats.SkillId && existingAttributeSkill.AttributeId == ats.AttributeId);
                    existingAttributeSkill.Value = updatedAttributeSkill.Value;
                }

                existing.AttributeSkills.RemoveAll(ats => !updatedSkill.AttributeSkills.Select(x => (x.SkillId, x.AttributeId)).Contains((ats.SkillId, ats.AttributeId)));
                existing.AttributeSkills.AddRange(updatedSkill.AttributeSkills.Where(ats => !existing.AttributeSkills.Select(x => (x.SkillId, x.AttributeId)).Contains((ats.SkillId, ats.AttributeId))));

                this.context.Skills.Update(existing);
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
