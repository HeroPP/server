﻿using Hero.Server.Core.Models;
using Attribute = Hero.Server.Core.Models.Attribute;

namespace Hero.Server.Core.Repositories
{
    public interface IAttributeRepository
    {
        Task<Attribute?> GetAttributeByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Attribute>> GetAllAttributesAsync(CancellationToken cancellationToken = default);
        Task CreateAttributeAsync(Attribute attribute, CancellationToken cancellationToken = default);
        Task UpdateAttributeAsync(Guid id, Attribute updatedAttribute, CancellationToken cancellationToken = default);
        Task DeleteAttributeAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
