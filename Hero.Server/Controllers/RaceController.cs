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
    public class RacesController : HeroControllerBase
    {
        private readonly IRaceRepository repository;
        private readonly IMapper mapper;

        public RacesController(IRaceRepository repository, IMapper mapper, ILogger<RacesController> logger) 
            : base(logger)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public Task<IActionResult> GetRaceByIdAsync(Guid id)
        {
            return this.HandleExceptions(async () =>
            {
                Race? race = await this.repository.GetRaceByIdAsync(id, this.HttpContext.User.GetUserId());
                if (race != null)
                {
                    return this.Ok(new RaceResponse(race, this.mapper));
                }

                return this.BadRequest();
            });
        }

        [HttpGet]
        public Task<IActionResult> GetAllRacesAsync()
        {
            return this.HandleExceptions(async () =>
            {
                List<Race> abilities = (await this.repository.GetAllRacesAsync(this.HttpContext.User.GetUserId())).ToList();

                return this.Ok(abilities.Select(race => new RaceResponse(race, this.mapper)).ToList());
            });
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteRaceAsync(Guid id)
        {
            return this.HandleExceptions(async () =>
            {
                await this.repository.DeleteRaceAsync(id, this.HttpContext.User.GetUserId());
                return this.Ok();
            });
        }

        [HttpPut("{id}")]
        public Task<IActionResult> UpdateRaceAsync(Guid id, [FromBody] CreateRaceRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Race race = this.mapper.Map<Race>(request);
                await this.repository.UpdateRaceAsync(id, race, this.HttpContext.User.GetUserId());
                return this.Ok(new RaceResponse(race, this.mapper));
            });
        }

        [HttpPost]
        public Task<IActionResult> CreateRaceAsync([FromBody] CreateRaceRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Race race = this.mapper.Map<Race>(request);
                await this.repository.CreateRaceAsync(race, this.HttpContext.User.GetUserId());

                return this.Ok(new RaceResponse(race, this.mapper));
            });
        }

    }
}
