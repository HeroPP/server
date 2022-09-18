using Hero.Server.Core.Models;

namespace Hero.Server.Core.Repositories
{
    public interface ICharacterRepository
    {
        Task<IEnumerable<Character>> GetAllCharactersByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task CreateCharacterAsync(Character character, Guid userId, CancellationToken cancellationToken = default);
        Task UpdateCharacterAsync(Guid id, Character updatedCharacter, Guid userId, CancellationToken cancellationToken = default);
        Task DeleteCharacterAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
        Task<Character?> GetCharacterByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Character>> GetAllCharactersAsync(CancellationToken cancellationToken = default);
        Task<Character?> GetCharacterWithNestedByIdAsync(Guid id, CancellationToken? cancellationToken = null);
    }
}
