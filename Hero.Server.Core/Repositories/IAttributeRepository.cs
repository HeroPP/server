using Hero.Server.Core.Models;
using Attribute = Hero.Server.Core.Models.Attribute;

namespace Hero.Server.Core.Repositories
{
    public interface IAttributeRepository
    {
        Task<Attribute?> GetAttributeByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Attribute>> GetAllAttributesAsync(Guid userId, CancellationToken cancellationToken = default);
        Task CreateAttributeAsync(Attribute attribute, Guid userId, CancellationToken cancellationToken = default);
        Task UpdateAttributeAsync(Guid id, Attribute updatedAttribute, Guid userId, CancellationToken cancellationToken = default);
        Task DeleteAttributeAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    }
}
