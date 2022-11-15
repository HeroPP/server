﻿using AutoMapper;

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
        public Task<IActionResult> GetCharacterDetailByIdAsync(Guid id, CancellationToken token)
        {
            return this.HandleExceptions(async () =>
            {
                Character? character = await this.repository.GetCharacterWithNestedByIdAsync(id, token);
                if (character != null)
                {
                    return this.Ok(this.mapper.Map<CharacterDetailResponse>(character));
                }

                return this.BadRequest();
            });
        }

        [HttpGet]
        public Task<IActionResult> GetCharacterOverviewsAsync(CancellationToken token)
        {
            return this.HandleExceptions(async () =>
            {
                List<Character> characters;
                bool isAdministrator = this.HttpContext.User.IsInRole(RoleNames.Administrator);
                if (isAdministrator)
                {
                    characters = await this.repository.GetAllCharactersAsync(token);
                }
                else
                {
                    Guid userId = this.HttpContext.User.GetUserId();
                    characters = (await this.repository.GetAllCharactersByUserIdAsync(userId, token)).ToList();
                }

                return this.Ok(characters.Select(character => this.mapper.Map<CharacterOverviewResponse>(character)).ToList());
            });
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteCharacterAsync(Guid id, CancellationToken token)
        {
            return this.HandleExceptions(async () =>
            {
                await this.repository.DeleteCharacterAsync(id, this.HttpContext.User.GetUserId(), token);
                return this.Ok();
            });
        }

        [HttpPut("{id}")]
        public Task<IActionResult> UpdateCharacterAsync(Guid id, [FromBody] CharacterRequest request, CancellationToken token)
        {
            return this.HandleExceptions(async () =>
            {
                Character character = this.mapper.Map<Character>(request);
                await this.repository.UpdateCharacterAsync(id, character, this.HttpContext.User.GetUserId(), token);
                return this.Ok();
            });
        }

        [HttpPost]
        public Task<IActionResult> CreateCharacterAsync([FromBody] CharacterRequest request, CancellationToken token)
        {
            return this.HandleExceptions(async () =>
            {
                Character character = this.mapper.Map<Character>(request);
                await this.repository.CreateCharacterAsync(character, this.HttpContext.User.GetUserId(), token);

                return this.Ok(this.mapper.Map<CreateCharacterResponse>(character));
            });
        }
    }
}
