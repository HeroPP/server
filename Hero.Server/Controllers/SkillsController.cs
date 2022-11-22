using AutoMapper;
using Hero.Server.Core.Models;
using Hero.Server.Core.Repositories;
using Hero.Server.Identity;
using Hero.Server.Messages.Requests;
using Hero.Server.Messages.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Attribute = Hero.Server.Core.Models.Attribute;

namespace Hero.Server.Controllers
{
    [ApiController, Authorize(Roles = RoleNames.Administrator), Route("api/[controller]")]
    public class SkillsController : HeroControllerBase
    {
        private readonly ISkillRepository repository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public SkillsController(ISkillRepository repository, IUserRepository userRepository, IMapper mapper, ILogger<SkillsController> logger)
            : base(logger)
        {
            this.repository = repository;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [Route("/error"), ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HandleError() => this.HandleErrors();

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSkillByIdAsync(Guid id, CancellationToken token)
        {
            await this.userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId(), token);
            Skill? skill = await this.repository.GetSkillByIdAsync(id, token);
            if (skill != null)
            {
                SkillResponse value = this.mapper.Map<SkillResponse>(skill);
                return this.Ok(value);
            }

            return this.NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSkillsAsync(CancellationToken token)
        {
            await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId(), token);
            List<Skill> skills = (await this.repository.GetAllSkillsAsync(token)).ToList();

            return this.Ok(skills.Select(skill => this.mapper.Map<SkillResponse>(skill)).ToList());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSkillAsync(Guid id, CancellationToken token)
        {
            await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
            await this.repository.DeleteSkillAsync(id, token);
            return this.Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSkillAsync(Guid id, [FromBody] SkillRequest request, CancellationToken token)
        {
            await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());

            Skill skill = this.mapper.Map<Skill>(request);
            await this.repository.UpdateSkillAsync(id, skill, token);
            skill = await this.repository.GetSkillByIdAsync(id);

            return this.Ok(this.mapper.Map<SkillResponse>(skill));
        }

        [HttpPost]
        public async Task<IActionResult> CreateSkillAsync([FromBody] SkillRequest request, CancellationToken token)
        {
            await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());

            Skill skill = this.mapper.Map<Skill>(request);
            await this.repository.CreateSkillAsync(skill, token);
            skill = await this.repository.GetSkillByIdAsync(skill.Id);

            return this.Ok(this.mapper.Map<SkillResponse>(skill));
        }

    }
}
