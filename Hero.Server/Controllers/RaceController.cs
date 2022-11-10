using AutoMapper;
using Hero.Server.Core.Models;
using Hero.Server.Core.Repositories;
using Hero.Server.DataAccess.Repositories;
using Hero.Server.Identity;
using Hero.Server.Messages.Requests;
using Hero.Server.Messages.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hero.Server.Controllers
{
    [ApiController, Authorize(Roles = RoleNames.User), Route("api/[controller]")]
    public class RacesController : HeroControllerBase
    {
        private readonly IRaceRepository repository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public RacesController(IRaceRepository repository, IUserRepository userRepository, IMapper mapper, ILogger<RacesController> logger) 
            : base(logger)
        {
            this.repository = repository;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public Task<IActionResult> GetRaceByIdAsync(Guid id)
        {
            return this.HandleExceptions(async () =>
            {
                Race? race = await this.repository.GetRaceByIdAsync(id);
                if (race != null)
                {
                    return this.Ok(this.mapper.Map<RaceResponse>(race));
                }

                return this.BadRequest();
            });
        }

        [HttpGet]
        public Task<IActionResult> GetAllRacesAsync()
        {
            return this.HandleExceptions(async () =>
            {
                List<Race> races = (await this.repository.GetAllRacesAsync()).ToList();

                return this.Ok(races.Select(race => this.mapper.Map<RaceResponse>(race)).ToList());
            });
        }

        [HttpDelete("{id}"), Authorize(Roles = RoleNames.User)]
        public Task<IActionResult> DeleteRaceAsync(Guid id)
        {
            return this.HandleExceptions(async () =>
            {
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                await this.repository.DeleteRaceAsync(id);
                return this.Ok();
            });
        }

        [HttpPut("{id}"), Authorize(Roles = RoleNames.User)]
        public Task<IActionResult> UpdateRaceAsync(Guid id, [FromBody] CreateRaceRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Race race = this.mapper.Map<Race>(request);
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                await this.repository.UpdateRaceAsync(id, race);
                return this.Ok(this.mapper.Map<RaceResponse>(race));
            });
        }

        [HttpPost, Authorize(Roles = RoleNames.User)]
        public Task<IActionResult> CreateRaceAsync([FromBody] CreateRaceRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Race race = this.mapper.Map<Race>(request);
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                await this.repository.CreateRaceAsync(race);
                race = await this.repository.GetRaceByIdAsync(race.Id);
                return this.Ok(this.mapper.Map<RaceResponse>(race));
            });
        }

    }
}
