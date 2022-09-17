using Hero.Server.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hero.Server.Core.Repositories
{
    public interface IAbilityRepository
    {
        Task<IEnumerable<Ability>> GetAllAbilitiesAsync(CancellationToken cancellationToken = default);
        Task CreateAbilityAsync(Ability ability, CancellationToken cancellationToken = default);
        Task UpdateAbilityAsync(string name, Ability updatedAbility, CancellationToken cancellationToken = default);
        Task DeleteAbilityAsync(string name, CancellationToken cancellationToken = default);
    }
}
