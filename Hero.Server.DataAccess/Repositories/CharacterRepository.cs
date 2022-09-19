using Hero.Server.Core;
using Hero.Server.Core.Exceptions;
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
        private readonly IUserRepository userRepository;
        private readonly ILogger<CharacterRepository> logger;

        public CharacterRepository(HeroDbContext context, IUserRepository userRepository, ILogger<CharacterRepository> logger)
        {
            this.context = context;
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public async Task<Character?> GetCharacterByIdAsync(Guid id, CancellationToken cancellationToken = default)
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

        public async Task<List<Character>> GetAllCharactersAsync(CancellationToken cancellationToken = default)
        {
            return await this.context.Characters.ToListAsync(cancellationToken);
        }

        public async Task<Character?> GetCharacterWithNestedByIdAsync(Guid id, CancellationToken? cancellationToken = default)
        {
            return await this.context.Characters
                .Include(c => c.NodeTrees)
                .ThenInclude(t => t.AllNodes)
                .ThenInclude(n => n.Skill)
                //.ThenInclude(s => s.Ability)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task CreateCharacterAsync(Character character, Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                User? user = await this.userRepository.GetUserByIdAsync(userId, cancellationToken);
                character.UserId = user!.Id;
                
                if (user.GroupId == null)
                {
                    throw new BaseException((int)EventIds.CannotCreateCharacterOutsideOfAGroup, "A character cannot be created without a associated group.");
                }

                character.GroupId = user.GroupId.Value;


                await this.context.Characters.AddAsync(character, cancellationToken);
                await this.context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
        }

        public async Task DeleteCharacterAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                Character? existing = await this.GetCharacterByIdAsync(id, cancellationToken);

                if(null == existing || userId != existing.UserId)
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

        public async Task UpdateCharacterAsync(Guid id, Character updatedCharacter, Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                Character? existing = await this.GetCharacterByIdAsync(id, cancellationToken);

                if (null == existing || userId != existing.UserId)
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
