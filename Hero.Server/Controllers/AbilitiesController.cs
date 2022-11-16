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
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public AbilitiesController(IAbilityRepository repository, IUserRepository userRepository, IMapper mapper, ILogger<AbilitiesController> logger) 
            : base(logger)
        {
            this.repository = repository;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [HttpGet("{name}")]
        public Task<IActionResult> GetAbilityByIdAsync(string name, CancellationToken token)
        {
            return this.HandleExceptions(async () =>
            {
                Ability ability = await this.repository.GetAbilityByNameAsync(name, token);
                return this.Ok(this.mapper.Map<AbilityResponse>(ability));
            });
        }

        [HttpGet]
        public Task<IActionResult> GetAllAbilitiesAsync(CancellationToken token)
        {
            return this.HandleExceptions(async () =>
            {
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                List<Ability> abilities = await this.repository.GetAllAbilitiesAsync(token);

                return this.Ok(abilities.Select(ability => this.mapper.Map<AbilityResponse>(ability)).ToList());
            });
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteAbilityAsync(Guid id, CancellationToken token)
        {
            return this.HandleExceptions(async () =>
            {
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                await this.repository.DeleteAbilityAsync(id, token);
                return this.Ok();
            });
        }

        [HttpPut("{id}")]
        public Task<IActionResult> UpdateAbilityAsync(Guid id, [FromBody] AbilityRequest request, CancellationToken token)
        {
            return this.HandleExceptions(async () =>
            {
                Ability ability = this.mapper.Map<Ability>(request);

                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                await this.repository.UpdateAbilityAsync(id, ability, token);

                return this.Ok(this.mapper.Map<AbilityResponse>(ability));
            });
        }

        [HttpPost]
        public Task<IActionResult> CreateAbilityAsync([FromBody] AbilityRequest request, CancellationToken token)
        {
            return this.HandleExceptions(async () =>
            {
                Ability ability = this.mapper.Map<Ability>(request);

                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                await this.repository.CreateAbilityAsync(ability, token);

                return this.Ok(this.mapper.Map<AbilityResponse>(ability));
            });
        }
    }
}
