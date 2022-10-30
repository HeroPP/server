using AutoMapper;

using Hero.Server.Core.Models;
using Hero.Server.Core.Repositories;
using Hero.Server.Identity;
using Hero.Server.Messages.Requests;
using Hero.Server.Messages.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Hero.Server.Controllers
{
    [ApiController, Authorize(Roles = RoleNames.User), Route("api/[controller]")]
    public class CharactersController : HeroControllerBase
    {
        private readonly ICharacterRepository repository;
        private readonly IMapper mapper;

        public CharactersController(ICharacterRepository repository, IMapper mapper, ILogger<CharactersController> logger)
            : base(logger)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public Task<IActionResult> GetCharacterDetailByIdAsync(Guid id)
        {
            return this.HandleExceptions(async () =>
            {
                Character? character = await this.repository.GetCharacterWithNestedByIdAsync(id);
                if (character != null)
                {
                    return this.Ok(new CharacterDetailResponse(character, mapper));
                }

                return this.BadRequest();
            });
        }

        [HttpGet]
        public Task<IActionResult> GetCharacterOverviewsAsync()
        {
            return this.HandleExceptions(async () =>
            {
                List<Character> characters;
                bool isAdministrator = this.HttpContext.User.IsInRole(RoleNames.Administrator);
                if (isAdministrator)
                {
                    characters = await this.repository.GetAllCharactersAsync();
                }
                else
                {
                    Guid userId = this.HttpContext.User.GetUserId();
                    characters = (await this.repository.GetAllCharactersByUserIdAsync(userId)).ToList();
                }

                return this.Ok(characters.Select(character => this.mapper.Map<CharacterOverviewResponse>(character)).ToList());
            });
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteCharacterAsync(Guid id)
        {
            return this.HandleExceptions(async () =>
            {
                await this.repository.DeleteCharacterAsync(id, this.HttpContext.User.GetUserId());
                return this.Ok();
            });
        }

        [HttpPut("{id}")]
        public Task<IActionResult> UpdateCharacterAsync(Guid id, [FromBody] CreateCharacterRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Character character = this.mapper.Map<Character>(request);
                await this.repository.UpdateCharacterAsync(id, character, this.HttpContext.User.GetUserId());
                return this.Ok();
            });
        }

        [HttpPost]
        public Task<IActionResult> CreateCharacterAsync([FromBody] CreateCharacterRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Character character = this.mapper.Map<Character>(request);
                await this.repository.CreateCharacterAsync(character, this.HttpContext.User.GetUserId());

                return this.Ok(this.mapper.Map<CreateCharacterResponse>(character));
            });
        }
    }
}
