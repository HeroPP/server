using AutoMapper;
using Hero.Server.Core.Models;
using Hero.Server.Core.Repositories;
using Hero.Server.DataAccess.Repositories;
using Hero.Server.Identity;
using Hero.Server.Messages.Requests;
using Hero.Server.Messages.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Attribute = Hero.Server.Core.Models.Attribute;

namespace Hero.Server.Controllers
{
    [ApiController, Authorize(Roles = RoleNames.Administrator), Route("api/[controller]")]
    public class AttributesController : HeroControllerBase
    {
        private readonly IAttributeRepository repository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public AttributesController(IAttributeRepository repository, IUserRepository userRepository, IMapper mapper, ILogger<AttributesController> logger)
            : base(logger)
        {
            this.repository = repository;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public Task<IActionResult> GetAttributeByIdAsync(Guid id)
        {
            return this.HandleExceptions(async () =>
            {
                Attribute? attribute = await this.repository.GetAttributeByIdAsync(id);
                if (attribute != null)
                {
                    return this.Ok(this.mapper.Map<AttributeResponse>(attribute));
                }

                return this.BadRequest();
            });
        }

        [HttpGet]
        public Task<IActionResult> GetAllAttributesAsync()
        {
            return this.HandleExceptions(async () =>
            {
                List<Attribute> abilities = (await this.repository.GetAllAttributesAsync()).ToList();

                return this.Ok(abilities.Select(attribute => this.mapper.Map<AttributeResponse>(attribute)).ToList());
            });
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteAttributeAsync(Guid id)
        {
            return this.HandleExceptions(async () =>
            {
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                await this.repository.DeleteAttributeAsync(id);
                return this.Ok();
            });
        }

        [HttpPut("{id}")]
        public Task<IActionResult> UpdateAttributeAsync(Guid id, [FromBody] AttributeRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Attribute attribute = this.mapper.Map<Attribute>(request);
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                await this.repository.UpdateAttributeAsync(id, attribute);
                return this.Ok(this.mapper.Map<AttributeResponse>(attribute));
            });
        }

        [HttpPost]
        public Task<IActionResult> CreateAttributeAsync([FromBody] AttributeRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Attribute attribute = this.mapper.Map<Attribute>(request);
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                await this.repository.CreateAttributeAsync(attribute);

                return this.Ok(this.mapper.Map<AttributeResponse>(attribute));
            });
        }

    }
}
