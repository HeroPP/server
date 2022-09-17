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
        private readonly ILogger<AbilityRepository> logger;

        public AbilityRepository(HeroDbContext context, ILogger<AbilityRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task CreateAbilityAsync(Ability ability, CancellationToken cancellationToken = default)
        {
            try
            {
                await this.context.Abilities.AddAsync(ability, cancellationToken);
                await this.context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
        }

        public async Task DeleteAbilityAsync(string name, CancellationToken cancellationToken = default)
        {
            try
            {
                Ability? existing = await GetAbilityByNameAsync(name, cancellationToken);
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

        public async Task<IEnumerable<Ability>> GetAllAbilitiesAsync(CancellationToken cancellationToken = default)
        {
            return await this.context.Abilities.ToListAsync(cancellationToken);
        }

        public async Task<Ability?> GetAbilityByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await this.context.Abilities.FindAsync(new object[] { name }, cancellationToken);
        }

        public async Task UpdateAbilityAsync(string name, Ability updatedAbility, CancellationToken cancellationToken = default)
        {
            try
            {
                Ability? existing = await GetAbilityByNameAsync(name, cancellationToken);

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
