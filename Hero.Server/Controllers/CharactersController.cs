using AutoMapper;

using Hero.Server.Core.Models;
using Hero.Server.DataAccess.Repositories;
using Hero.Server.Messages.Requests;
using Hero.Server.Messages.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hero.Server.Controllers
{
    [ApiController, Authorize(Roles = "Hero,Hero Administrator"), Route("api/[controller]")]
    public class CharactersController : HeroControllerBase
    {
        private readonly CharacterRepository repository;
        private readonly IMapper mapper;
        private readonly ILogger<CharactersController> logger;

        public CharactersController(CharacterRepository repository, IMapper mapper, ILogger<CharactersController> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        public Task<Response<CharacterDetailResponse?>> GetCharacterDetailByIdAsync(Guid id)
        {
            return this.HandleExceptions(async () =>
            {
                Character? character = await this.repository.GetCharacterNestedByIdAsync(id);
                if (character != null)
                {
                    return new CharacterDetailResponse(character);
                }

                return null;
            });
        }

        [HttpGet]
        public Task<Response<List<CharacterOverviewResponse>>> GetUserCharacterOverviewsAsync()
        {
            return this.HandleExceptions(async () =>
            {
                //Todo getCurrentUser
                Guid userId = Guid.NewGuid();
                List<Character> characters = (await this.repository.GetAllCharactersByUserIdAsync(userId)).ToList();

                return characters.Select(character => this.mapper.Map<CharacterOverviewResponse>(character)).ToList();
            });
        }

        [HttpGet]
        public Task<Response<List<CharacterOverviewResponse>>> GetAllCharacterOverviewsAsync()
        {
            return this.HandleExceptions(async () =>
            {
                List<Character> characters = (await this.repository.GetAllCharactersAsync()).ToList();

                return characters.Select(character => this.mapper.Map<CharacterOverviewResponse>(character)).ToList();
            });
        }

        [HttpDelete("{id}")]
        public Task<Response> DeleteCharacterAsync(Guid id)
        {
            return this.HandleExceptions(async () =>
            {
                await this.repository.DeleteCharacterAsync(id);
            });
        }

        [HttpPut("{id}")]
        public Task<Response> UpdateCharacterAsync(Guid id, [FromBody] CreateCharacterRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Character character = this.mapper.Map<Character>(request);
                await this.repository.UpdateCharacterAsync(id, character);
            });
        }

        [HttpPost]
        public Task<Response<CreateCharacterResponse>> CreateCharacterAsync([FromBody] CreateCharacterRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Character character = this.mapper.Map<Character>(request);
                await this.repository.CreateCharacterAsync(character);

                return this.mapper.Map<CreateCharacterResponse>(character);
            });
        }
    }
}
