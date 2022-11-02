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
    public class SkilltreesController : HeroControllerBase
    {
        private readonly ISkilltreeRepository repository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public SkilltreesController(ISkilltreeRepository repository, IUserRepository userRepository, IMapper mapper, ILogger<SkilltreesController> logger)
            : base(logger)
        {
            this.repository = repository;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public Task<IActionResult> GetSkilltreeByIdAsync(Guid id)
        {
            return this.HandleExceptions(async () =>
            {
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                Skilltree? tree = await this.repository.GetSkilltreeByIdAsync(id);
                if (tree != null)
                {
                    return this.Ok(this.mapper.Map<SkilltreeResponse>(tree));
                }

                return this.BadRequest();
            });
        }

        [HttpGet("{characterId}")]
        public Task<IActionResult> GetSkilltreeOverviewsAsync(Guid characterId)
        {
            return this.HandleExceptions(async () =>
            {
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                List<Skilltree> tree = (await this.repository.GetAllSkilltreesOfCharacterAsync(characterId)).ToList();

                return this.Ok(tree.Select(nodeTree => this.mapper.Map<SkilltreeOverviewResponse>(nodeTree)).ToList());
            });
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteskilltreeAsync(Guid id)
        {
            return this.HandleExceptions(async () =>
            {
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                await this.repository.DeleteSkilltreeAsync(id);
                return this.Ok();
            });
        }

        [HttpPut("{id}")]
        public Task<IActionResult> UpdateSkilltreeAsync(Guid id, [FromBody] CreateSkilltreeRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Skilltree tree = this.mapper.Map<Skilltree>(request);
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                await this.repository.UpdateSkilltreeAsync(id, tree);
                return this.Ok();
            });
        }

        [HttpPost]
        public Task<IActionResult> CreateSkilltreeAsync([FromBody] CreateSkilltreeRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Skilltree tree = this.mapper.Map<Skilltree>(request);
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                await this.repository.CreateSkilltreeAsync(tree);

                return this.Ok(this.mapper.Map<SkilltreeResponse>(tree));
            });
        }
    }
}
