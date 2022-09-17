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

        [HttpGet, Authorize(Roles = "Hero Administrator")]
        public Task<Response<CharacterInfo?>> GetCharacterById([FromBody] GetCharacterRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Character? character = await this.repository.GetCharacterByIdAsync(request.Id);
                if (character != null)
                {
                    return new CharacterInfo(character);
                }

                return null;
            });
        }

        [HttpPost]
        public Task<Response<CharacterInfo>> CreateCharacter([FromBody] CreateCharacterRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Character character = this.mapper.Map<Character>(request);
                await this.repository.CreateCharacterAsync(character);

                return new CharacterInfo(character);
            });
        }
    }
}
