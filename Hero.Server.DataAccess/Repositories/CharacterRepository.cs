using Hero.Server.Core.Logging;
using Hero.Server.Core.Models;
using Hero.Server.Core.Repositories;
using Hero.Server.DataAccess.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hero.Server.DataAccess.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly HeroDbContext context;
        private readonly ILogger<CharacterRepository> logger;

        public CharacterRepository(HeroDbContext context, ILogger<CharacterRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        private async Task<Character?> GetCharacterById(Guid id, CancellationToken cancellationToken = default)
        {
            return await this.context.Characters.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<IEnumerable<Character>> GetAllCharactersByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await this.context
                .Characters
                .Where(c => c.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task CreateCharacterAsync(Character character, CancellationToken cancellationToken = default)
        {
            try
            {
                await this.context.Characters.AddAsync(character, cancellationToken);
                await this.context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
        }

        public async Task DeleteCharacterAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                Character? existing = await this.GetCharacterById(id, cancellationToken);

                if(null == existing)
                {
                    this.logger.LogCharacterDoesNotExist(id);
                    return;
                }

                this.context.Characters.Remove(existing);
                await this.context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
        }

        public async Task UpdateCharacterAsync(Guid id, Character updatedCharacter, CancellationToken cancellationToken = default)
        {
            try
            {
                Character? existing = await this.GetCharacterById(id, cancellationToken);

                if (null == existing)
                {
                    throw new Exception($"The character (id: {id}) you're trying to update does not exist.");
                }

                existing.Update(updatedCharacter);

                this.context.Characters.Update(existing);
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
