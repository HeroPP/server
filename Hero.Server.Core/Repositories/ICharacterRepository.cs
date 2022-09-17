using Hero.Server.Core.Models;

namespace Hero.Server.Core.Repositories
{
    public interface ICharacterRepository
    {
        Task<IEnumerable<Character>> GetAllCharactersByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task CreateCharacterAsync(Character character, CancellationToken cancellationToken = default);
        Task UpdateCharacterAsync(Guid id, Character updatedCharacter, CancellationToken cancellationToken = default);
        Task DeleteCharacterAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Character?> GetCharacterById(Guid id, CancellationToken? cancellationToken = null);
    }
}
