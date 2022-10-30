using Hero.Server.Core.Logging;
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
        private readonly IUserRepository userRepository;
        private readonly ILogger<SkillRepository> logger;

        public SkillRepository(HeroDbContext context, IUserRepository userRepository, ILogger<SkillRepository> logger)
        {
            this.context = context;
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public async Task<Skill?> GetSkillByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            User? user = await this.userRepository.GetUserByIdAsync(userId, cancellationToken);

            return await this.context.Skills.Where(s => s.GroupId == user!.OwnedGroup.Id).FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<AttributeSkill?> GetAttributeSkillByIdAsync(Guid id, Guid attributeId, Guid userId, CancellationToken cancellationToken = default)
        {
            User? user = await this.userRepository.GetUserByIdAsync(userId, cancellationToken);

            return await this.context.AttributeSkills.Where(s => s.Attribute.GroupId == user!.OwnedGroup.Id).FirstOrDefaultAsync(ats => ats.SkillId == id && ats.AttributeId == attributeId, cancellationToken);
        }

        public async Task CreateSkillAsync(Skill skill, Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                User? user = await this.userRepository.GetUserByIdAsync(userId, cancellationToken);
                if (null != user)
                {
                    skill.GroupId = user.OwnedGroup.Id;
                    await this.context.Skills.AddAsync(skill, cancellationToken);
                    await this.context.SaveChangesAsync(cancellationToken);
                }
                
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
                Skill? existing = await this.GetSkillByIdAsync(id, userId, cancellationToken);
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
            User? user = await this.userRepository.GetUserByIdAsync(userId, cancellationToken);
            return await this.context.Skills.Include(s => s.Ability).Where(s => s.GroupId == user!.OwnedGroup.Id).ToListAsync(cancellationToken);
        }

        public async Task UpdateSkillAsync(Guid id, Skill updatedSkill, Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                Skill? existing = await this.GetSkillByIdAsync(id, userId, cancellationToken);

                if (null == existing)
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
