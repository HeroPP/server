using AutoMapper;
using Hero.Server.Core.Models;
using Hero.Server.Core.Repositories;
using Hero.Server.Identity;
using Hero.Server.Messages.Requests;
using Hero.Server.Messages.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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

        [HttpGet("{id}")]
        public Task<IActionResult> GetSkillByIdAsync(Guid id, CancellationToken token)
        {
            return this.HandleExceptions(async () =>
            {
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                Skill? skill = await this.repository.GetSkillByIdAsync(id, token);
                if (skill != null)
                {
                    return this.Ok(this.mapper.Map<SkillResponse>(skill));
                }

                return this.BadRequest();
            });
        }

        [HttpGet]
        public Task<IActionResult> GetAllSkillsAsync(CancellationToken token)
        {
            return this.HandleExceptions(async () =>
            {
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                List<Skill> skills = (await this.repository.GetAllSkillsAsync(token)).ToList();

                return this.Ok(skills.Select(skill => this.mapper.Map<SkillResponse>(skill)).ToList());
            });
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteSkillAsync(Guid id, CancellationToken token)
        {
            return this.HandleExceptions(async () =>
            {
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                await this.repository.DeleteSkillAsync(id, token);
                return this.Ok();
            });
        }

        [HttpPut("{id}")]
        public Task<IActionResult> UpdateSkillAsync(Guid id, [FromBody] CreateSkillRequest request, CancellationToken token)
        {
            return this.HandleExceptions(async () =>
            {
                Skill skill = this.mapper.Map<Skill>(request);
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                await this.repository.UpdateSkillAsync(id, skill, token);
                return this.Ok(this.mapper.Map<SkillResponse>(skill));
            });
        }

        [HttpPost]
        public Task<IActionResult> CreateSkillAsync([FromBody] CreateSkillRequest request, CancellationToken token)
        {
            return this.HandleExceptions(async () =>
            {
                Skill skill = this.mapper.Map<Skill>(request);
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                await this.repository.CreateSkillAsync(skill, token);

                return this.Ok(this.mapper.Map<SkillResponse>(skill));
            });
        }

    }
}
