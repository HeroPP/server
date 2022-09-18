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
        private readonly ILogger<SkillsController> logger;

        public SkillsController(ISkillRepository repository, IMapper mapper, ILogger<SkillsController> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        public Task<Response<SkillResponse?>> GetSkillByIdAsync(Guid id)
        {
            return this.HandleExceptions(async () =>
            {
                Skill? skill = await this.repository.GetSkillByIdAsync(id);
                if (skill != null)
                {
                    return this.mapper.Map<SkillResponse>(skill);
                }

                return null;
            });
        }

        [HttpGet]
        public Task<Response<List<SkillResponse>>> GetAllSkillsAsync()
        {
            return this.HandleExceptions(async () =>
            {
                List<Skill> abilities = (await this.repository.GetAllSkillsAsync(this.HttpContext.User.GetUserId())).ToList();

                return abilities.Select(skill => this.mapper.Map<SkillResponse>(skill)).ToList();
            });
        }

        [HttpDelete("{id}")]
        public Task<Response> DeleteSkillAsync(Guid id)
        {
            return this.HandleExceptions(async () =>
            {
                await this.repository.DeleteSkillAsync(id, this.HttpContext.User.GetUserId());
            });
        }

        [HttpPut("{id}")]
        public Task<Response> UpdateSkillAsync(Guid id, [FromBody] CreateSkillRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Skill skill = this.mapper.Map<Skill>(request);
                await this.repository.UpdateSkillAsync(id, skill, this.HttpContext.User.GetUserId());
            });
        }

        [HttpPost]
        public Task<Response<SkillResponse>> CreateSkillAsync([FromBody] CreateSkillRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Skill skill = this.mapper.Map<Skill>(request);
                await this.repository.CreateSkillAsync(skill, this.HttpContext.User.GetUserId());

                return this.mapper.Map<SkillResponse>(skill);
            });
        }

    }
}
