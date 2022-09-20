using Hero.Server.Core.Logging;
using Hero.Server.Core.Models;
using Hero.Server.Core.Repositories;
using Hero.Server.DataAccess.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hero.Server.DataAccess.Repositories
{
    public class AbilityRepository : IAbilityRepository
    {
        private readonly HeroDbContext context;
        private readonly IUserRepository userRepository;
        private readonly ILogger<AbilityRepository> logger;

        public AbilityRepository(HeroDbContext context, IUserRepository userRepository, ILogger<AbilityRepository> logger)
        {
            this.context = context;
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public async Task<Ability?> GetAbilityByNameAsync(string name, Guid userId, CancellationToken cancellationToken = default)
        {
            User? user = await this.userRepository.GetUserByIdAsync(userId, cancellationToken);
            return await this.context.Abilities.Where(a => a.GroupId == user!.OwnedGroup.Id).FirstOrDefaultAsync(g => g.Name == name, cancellationToken);
        }

        public async Task CreateAbilityAsync(Ability ability, Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                User? user = await this.userRepository.GetUserByIdAsync(userId, cancellationToken);
                if (null != user)
                {
                    ability.GroupId = user.OwnedGroup.Id;
                    await this.context.Abilities.AddAsync(ability, cancellationToken);
                    await this.context.SaveChangesAsync(cancellationToken);
                }
                
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
        }

        public async Task DeleteAbilityAsync(string name, Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                Ability? existing = await this.GetAbilityByNameAsync(name, userId, cancellationToken);
                if(null == existing)
                {
                    this.logger.LogAbilityDoesNotExist(name);
                    return;
                }
                this.context.Abilities.Remove(existing);
                await this.context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
        }

        public async Task<IEnumerable<Ability>> GetAllAbilitiesAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            User? user = await this.userRepository.GetUserByIdAsync(userId, cancellationToken);
            return await this.context.Abilities.Where(a => a.GroupId == user!.OwnedGroup.Id).ToListAsync(cancellationToken);
        }

        public async Task UpdateAbilityAsync(string name, Ability updatedAbility, Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                Ability? existing = await this.GetAbilityByNameAsync(name, userId, cancellationToken);

                if (null == existing)
                {
                    throw new Exception($"The Ability (name: {name}) you're trying to update does not exist.");
                }

                existing.Update(updatedAbility);

                this.context.Abilities.Update(existing);
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
