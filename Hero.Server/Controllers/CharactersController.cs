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

        [Route("/error"), ApiExplorerSettings(IgnoreApi = true)]
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
    }
}
