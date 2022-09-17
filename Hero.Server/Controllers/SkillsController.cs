using AutoMapper;
using Hero.Server.Core.Models;
using Hero.Server.DataAccess.Repositories;
using Hero.Server.Messages.Requests;
using Hero.Server.Messages.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Hero.Server.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class SkillsController : HeroControllerBase
    {
        private readonly SkillRepository repository;
        private readonly IMapper mapper;
        private readonly ILogger<SkillsController> logger;

        public SkillsController(SkillRepository repository, IMapper mapper, ILogger<SkillsController> logger)
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
                List<Skill> abilities = (await this.repository.GetAllSkillsAsync()).ToList();

                return abilities.Select(skill => this.mapper.Map<SkillResponse>(skill)).ToList();
            });
        }

        [HttpDelete("{id}")]
        public Task<Response> DeleteSkillAsync(Guid id)
        {
            return this.HandleExceptions(async () =>
            {
                await this.repository.DeleteSkillAsync(id);
            });
        }

        [HttpPut("{id}")]
        public Task<Response> UpdateSkillAsync(Guid id, [FromBody] CreateSkillRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Skill skill = this.mapper.Map<Skill>(request);
                await this.repository.UpdateSkillAsync(id, skill);
            });
        }

        [HttpPost]
        public Task<Response<SkillResponse>> CreateSkillAsync([FromBody] CreateSkillRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Skill skill = this.mapper.Map<Skill>(request);
                await this.repository.CreateSkillAsync(skill);

                return this.mapper.Map<SkillResponse>(skill);
            });
        }

    }
}
