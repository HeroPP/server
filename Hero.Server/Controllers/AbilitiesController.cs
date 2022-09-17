using AutoMapper;
using Hero.Server.Core.Models;
using Hero.Server.DataAccess.Repositories;
using Hero.Server.Messages.Requests;
using Hero.Server.Messages.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Hero.Server.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class AbilitiesController : HeroControllerBase
    {
        private readonly AbilityRepository repository;
        private readonly IMapper mapper;
        private readonly ILogger<AbilitiesController> logger;

        public AbilitiesController(AbilityRepository repository, IMapper mapper, ILogger<AbilitiesController> logger)
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
                List<Ability> abilities = (await this.repository.GetAllAbilitiesAsync()).ToList();

                return abilities.Select(ability => this.mapper.Map<AbilityResponse>(ability)).ToList();
            });
        }

        [HttpDelete("{name}")]
        public Task<Response> DeleteAbilityAsync(string name)
        {
            return this.HandleExceptions(async () =>
            {
                await this.repository.DeleteAbilityAsync(name);
            });
        }

        [HttpPut("{name}")]
        public Task<Response> UpdateAbilityAsync(string name, [FromBody] CreateAbilityRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Ability ability = this.mapper.Map<Ability>(request);
                await this.repository.UpdateAbilityAsync(name, ability);
            });
        }

        [HttpPost]
        public Task<Response<AbilityResponse>> CreateAbilityAsync([FromBody] CreateAbilityRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Ability ability = this.mapper.Map<Ability>(request);
                await this.repository.CreateAbilityAsync(ability);

                return this.mapper.Map<AbilityResponse>(ability);
            });
        }

    }
}
