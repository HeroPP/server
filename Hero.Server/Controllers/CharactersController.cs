using AutoMapper;

using Hero.Server.Core.Exceptions;
using Hero.Server.Core.Models;
using Hero.Server.Core.Repositories;
using Hero.Server.Identity;
using Hero.Server.Messages.Requests;
using Hero.Server.Messages.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [ApiExplorerSettings(IgnoreApi = true), NonAction, Route("/error")]
        public IActionResult HandleError() => this.HandleErrors();

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCharacterDetailByIdAsync(Guid id, CancellationToken token)
        {
            Character? character = await this.repository.GetCharacterWithNestedByIdAsync(id, token);

            if (character != null)
            {
                return this.Ok(this.mapper.Map<CharacterDetailResponse>(character));
            }

            return this.NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetCharacterOverviewsAsync(CancellationToken token)
        {
            List<Character> characters = await this.repository.GetCharactersAsync(this.HttpContext.User.IsInRole(RoleNames.Administrator) ? null : this.HttpContext.User.GetUserId(), token);

            return this.Ok(characters.Select(character => this.mapper.Map<CharacterOverviewResponse>(character)).ToList());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacterAsync(Guid id, CancellationToken token)
        {
            await this.repository.DeleteCharacterAsync(id, this.HttpContext.User.GetUserId(), token);

            return this.Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCharacterAsync(Guid id, [FromBody] CharacterRequest request, CancellationToken token)
        {
            Character character = this.mapper.Map<Character>(request);

            await this.repository.UpdateCharacterAsync(id, character, this.HttpContext.User.GetUserId(), token);

            return this.Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCharacterAsync([FromBody] CharacterRequest request, CancellationToken token)
        {
            Character character = this.mapper.Map<Character>(request);

            await this.repository.CreateCharacterAsync(character, this.HttpContext.User.GetUserId(), token);

            return this.Ok(this.mapper.Map<CreateCharacterResponse>(character));
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateInventoryAsync(Guid id, [FromBody] CharacterUpdateRequest request, CancellationToken token)
        {
            Character? character = await this.repository.GetCharacterByIdAsync(id, token);

            if (null == character)
            {
                throw new ObjectNotFoundException("The character you are looking for could not be found.");
            }

            character.Name = request.Name ?? character.Name;
            character.Description = request.Description ?? character.Description;
            character.Age = request.Age ?? character.Age;
            character.Inventory = request.Inventory ?? character.Inventory;
            character.Religion = request.Religion ?? character.Religion;
            character.Religion = request.Religion ?? character.Religion;
            character.Relationship = request.Relationship ?? character.Relationship;
            character.Notes = request.Notes ?? character.Notes;
            character.Profession = request.Profession ?? character.Profession;
            character.IconUrl = request.IconUrl ?? character.IconUrl;

            await this.repository.UpdateCharacterAsync(id, character, this.HttpContext.User.GetUserId(), token);

            return this.Ok(this.mapper.Map<CreateCharacterResponse>(character));
        }
    }
}
