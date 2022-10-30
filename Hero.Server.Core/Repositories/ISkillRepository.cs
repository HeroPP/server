using Hero.Server.Core.Models;

namespace Hero.Server.Core.Repositories
{
    public interface ISkillRepository
    {
        Task<Skill?> GetSkillByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Skill>> GetAllSkillsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task CreateSkillAsync(Skill skill, Guid userId, CancellationToken cancellationToken = default);
        Task UpdateSkillAsync(Guid id, Skill updatedSkill, Guid userId, CancellationToken cancellationToken = default);
        Task DeleteSkillAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
        Task<AttributeSkill?> GetAttributeSkillByIdAsync(Guid id, Guid attributeId, Guid userId, CancellationToken cancellationToken = default);
    }
}
