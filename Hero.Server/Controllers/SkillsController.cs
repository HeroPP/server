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
        private readonly IMapper mapper;

        public SkillsController(ISkillRepository repository, IMapper mapper, ILogger<SkillsController> logger)
            : base(logger)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public Task<IActionResult> GetSkillByIdAsync(Guid id)
        {
            return this.HandleExceptions(async () =>
            {
                Skill? skill = await this.repository.GetSkillByIdAsync(id, this.HttpContext.User.GetUserId());
                if (skill != null)
                {
                    return this.Ok(this.mapper.Map<SkillResponse>(skill));
                }

                return this.BadRequest();
            });
        }

        [HttpGet]
        public Task<IActionResult> GetAllSkillsAsync()
        {
            return this.HandleExceptions(async () =>
            {
                List<Skill> abilities = (await this.repository.GetAllSkillsAsync(this.HttpContext.User.GetUserId())).ToList();

                return this.Ok(abilities.Select(skill => this.mapper.Map<SkillResponse>(skill)).ToList());
            });
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteSkillAsync(Guid id)
        {
            return this.HandleExceptions(async () =>
            {
                await this.repository.DeleteSkillAsync(id, this.HttpContext.User.GetUserId());
                return this.Ok();
            });
        }

        [HttpPut("{id}")]
        public Task<IActionResult> UpdateSkillAsync(Guid id, [FromBody] CreateSkillRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Skill skill = this.mapper.Map<Skill>(request);
                await this.repository.UpdateSkillAsync(id, skill, this.HttpContext.User.GetUserId());
                return this.Ok(this.mapper.Map<SkillResponse>(skill));
            });
        }

        [HttpPost]
        public Task<IActionResult> CreateSkillAsync([FromBody] CreateSkillRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Skill skill = this.mapper.Map<Skill>(request);
                await this.repository.CreateSkillAsync(skill, this.HttpContext.User.GetUserId());

                return this.Ok(this.mapper.Map<SkillResponse>(skill));
            });
        }

    }
}
