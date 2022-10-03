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

        public AbilitiesController(IAbilityRepository repository, IMapper mapper, ILogger<AbilitiesController> logger) 
            : base(logger)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet("{name}")]
        public Task<IActionResult> GetAbilityByIdAsync(string name)
        {
            return this.HandleExceptions(async () =>
            {
                Ability? ability = await this.repository.GetAbilityByNameAsync(name, this.HttpContext.User.GetUserId());
                if (ability != null)
                {
                    return this.Ok(this.mapper.Map<AbilityResponse>(ability));
                }

                return this.BadRequest();
            });
        }

        [HttpGet]
        public Task<IActionResult> GetAllAbilitiesAsync()
        {
            return this.HandleExceptions(async () =>
            {
                List<Ability> abilities = (await this.repository.GetAllAbilitiesAsync(this.HttpContext.User.GetUserId())).ToList();

                return this.Ok(abilities.Select(ability => this.mapper.Map<AbilityResponse>(ability)).ToList());
            });
        }

        [HttpDelete("{name}")]
        public Task<IActionResult> DeleteAbilityAsync(string name)
        {
            return this.HandleExceptions(async () =>
            {
                await this.repository.DeleteAbilityAsync(name, this.HttpContext.User.GetUserId());
                return this.Ok();
            });
        }

        [HttpPut("{name}")]
        public Task<IActionResult> UpdateAbilityAsync(string name, [FromBody] CreateAbilityRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Ability ability = this.mapper.Map<Ability>(request);
                await this.repository.UpdateAbilityAsync(name, ability, this.HttpContext.User.GetUserId());
                return this.Ok(this.mapper.Map<AbilityResponse>(ability));
            });
        }

        [HttpPost]
        public Task<IActionResult> CreateAbilityAsync([FromBody] CreateAbilityRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Ability ability = this.mapper.Map<Ability>(request);
                await this.repository.CreateAbilityAsync(ability, this.HttpContext.User.GetUserId());

                return this.Ok(this.mapper.Map<AbilityResponse>(ability));
            });
        }

    }
}
