﻿using Hero.Server.Core.Models;

namespace Hero.Server.Core.Repositories
{
    public interface ISkillRepository
    {
        Task<IEnumerable<Skill>> GetAllSkillsAsync(CancellationToken cancellationToken = default);
        Task CreateSkillAsync(Skill skill, CancellationToken cancellationToken = default);
        Task UpdateSkillAsync(Guid id, Skill updatedSkill, CancellationToken cancellationToken = default);
        Task DeleteSkillAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
