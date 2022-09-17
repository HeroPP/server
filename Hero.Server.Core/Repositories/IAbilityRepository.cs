using Hero.Server.Core.Models;

namespace Hero.Server.Core.Repositories
{
    public interface IAbilityRepository
    {
        Task<Ability?> GetAbilityByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<IEnumerable<Ability>> GetAllAbilitiesAsync(CancellationToken cancellationToken = default);
        Task CreateAbilityAsync(Ability ability, CancellationToken cancellationToken = default);
        Task UpdateAbilityAsync(string name, Ability updatedAbility, CancellationToken cancellationToken = default);
        Task DeleteAbilityAsync(string name, CancellationToken cancellationToken = default);
    }
}
