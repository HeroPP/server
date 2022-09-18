using Hero.Server.Core.Logging;
using Hero.Server.Core.Models;
using Hero.Server.Core.Repositories;
using Hero.Server.DataAccess.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hero.Server.DataAccess.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly HeroDbContext context;
        private readonly ILogger<SkillRepository> logger;

        public SkillRepository(HeroDbContext context, ILogger<SkillRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task CreateSkillAsync(Skill skill, Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                skill.UserId = userId;
                await this.context.Skills.AddAsync(skill, cancellationToken);
                await this.context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
        }

        public async Task DeleteSkillAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
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

        public async Task<IEnumerable<Skill>> GetAllSkillsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await this.context.Skills.Where(s => s.UserId == userId).ToListAsync(cancellationToken);
        }

        public async Task<Skill?> GetSkillByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await this.context.Skills.FindAsync(new object[] { id }, cancellationToken);
        }
        
        public async Task UpdateSkillAsync(Guid id, Skill updatedSkill, Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                Skill? existing = await this.GetSkillByIdAsync(id, cancellationToken);

                if (null == existing || existing.UserId != userId)
                {
                    throw new Exception($"The Skill (id: {id}) you're trying to update does not exist.");
                }

                existing.Update(updatedSkill);

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
