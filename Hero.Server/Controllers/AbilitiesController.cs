using AutoMapper;
using Hero.Server.Core.Models;
using Hero.Server.Core.Repositories;
using Hero.Server.Identity;
using Hero.Server.Messages.Requests;
using Hero.Server.Messages.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hero.Server.Controllers
{
    [ApiController, Authorize(Roles = RoleNames.Administrator), Route("api/[controller]")]
    public class AbilitiesController : HeroControllerBase
    {
        private readonly IAbilityRepository repository;
        private readonly IMapper mapper;
        private readonly ILogger<AbilitiesController> logger;

        public AbilitiesController(IAbilityRepository repository, IMapper mapper, ILogger<AbilitiesController> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet("{name}")]
        public Task<Response<AbilityResponse?>> GetAbilityByIdAsync(string name)
        {
            return this.HandleExceptions(async () =>
            {
                Ability? ability = await this.repository.GetAbilityByNameAsync(name);
                if (ability != null)
                {
                    return this.mapper.Map<AbilityResponse>(ability);
                }

                return null;
            });
        }

        [HttpGet]
        public Task<Response<List<AbilityResponse>>> GetAllAbilitiesAsync()
        {
            return this.HandleExceptions(async () =>
            {
                List<Ability> abilities = (await this.repository.GetAllAbilitiesAsync(this.HttpContext.User.GetUserId())).ToList();

                return abilities.Select(ability => this.mapper.Map<AbilityResponse>(ability)).ToList();
            });
        }

        [HttpDelete("{name}")]
        public Task<Response> DeleteAbilityAsync(string name)
        {
            return this.HandleExceptions(async () =>
            {
                await this.repository.DeleteAbilityAsync(name, this.HttpContext.User.GetUserId());
            });
        }

        [HttpPut("{name}")]
        public Task<Response> UpdateAbilityAsync(string name, [FromBody] CreateAbilityRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Ability ability = this.mapper.Map<Ability>(request);
                await this.repository.UpdateAbilityAsync(name, ability, this.HttpContext.User.GetUserId());
            });
        }

        [HttpPost]
        public Task<Response<AbilityResponse>> CreateAbilityAsync([FromBody] CreateAbilityRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Ability ability = this.mapper.Map<Ability>(request);
                await this.repository.CreateAbilityAsync(ability, this.HttpContext.User.GetUserId());

                return this.mapper.Map<AbilityResponse>(ability);
            });
        }

    }
}
