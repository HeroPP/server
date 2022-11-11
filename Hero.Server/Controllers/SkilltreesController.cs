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
        public async Task<IActionResult> GetSkilltreeByIdAsync(Guid id)
        {
            return await this.HandleExceptions(async () =>
            {
                await this.userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                Skilltree? tree = await this.repository.GetSkilltreeByIdAsync(id);
                if (tree != null)
                {
                    SkilltreeResponse value = this.mapper.Map<SkilltreeResponse>(tree);
                    return this.Ok(value);
                }

                return this.NotFound();
            });
        }

        //[HttpGet("{characterId}")]
        //public Task<IActionResult> GetSkilltreeOverviewsForCharacterAsync(Guid characterId, CancellationToken token)
        //{
        //    return this.HandleExceptions(async () =>
        //    {
        //        await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
        //        List<Skilltree> trees = (await this.repository.FilterSkilltrees(characterId, token)).ToList();

        //        return this.Ok(trees.Select(skilltrees => this.mapper.Map<SkilltreeOverviewResponse>(skilltrees)).ToList());
        //    });
        //}

        [HttpGet]
        public async Task<IActionResult> GetSkilltreeOverviewsAsync(CancellationToken token)
        {
            return await this.HandleExceptions(async () =>
            {
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                List<Skilltree> trees = (await this.repository.FilterSkilltrees(null, token)).ToList();

                return this.Ok(trees.Select(skilltrees => this.mapper.Map<SkilltreeOverviewResponse>(skilltrees)).ToList());
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteskilltreeAsync(Guid id)
        {
            return await this.HandleExceptions(async () =>
            {
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                await this.repository.DeleteSkilltreeAsync(id);
                return this.Ok();
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSkilltreeAsync(Guid id, [FromBody] CreateSkilltreeRequest request)
        {
            return await this.HandleExceptions(async () =>
            {
                Skilltree tree = this.mapper.Map<Skilltree>(request);
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                await this.repository.UpdateSkilltreeAsync(id, tree);
                return this.Ok();
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateSkilltreeAsync([FromBody] CreateSkilltreeRequest request)
        {
            return await this.HandleExceptions(async () =>
            {
                Skilltree tree = this.mapper.Map<Skilltree>(request);
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                await this.repository.CreateSkilltreeAsync(tree);

                return this.Ok(this.mapper.Map<SkilltreeResponse>(tree));
            });
        }
    }
}
