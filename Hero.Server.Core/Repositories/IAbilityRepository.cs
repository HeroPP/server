using Hero.Server.Core.Models;

namespace Hero.Server.Core.Repositories
{
    public interface IAbilityRepository
    {
        Task<IEnumerable<Ability>> GetAllAbilitiesAsync(Guid userId, CancellationToken cancellationToken = default);
        Task CreateAbilityAsync(Ability ability, Guid userId, CancellationToken cancellationToken = default);
        Task UpdateAbilityAsync(string name, Ability updatedAbility, Guid userId, CancellationToken cancellationToken = default);
        Task DeleteAbilityAsync(string name, Guid userId, CancellationToken cancellationToken = default);
    }
}
