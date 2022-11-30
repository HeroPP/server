using AutoMapper;

using Hero.Server.Core.Repositories;
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

        [ApiExplorerSettings(IgnoreApi = true), NonAction, Route("/error")]
        public IActionResult HandleError() => this.HandleErrors();

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAttributeByIdAsync(Guid id)
        {
            Attribute? attribute = await this.repository.GetAttributeByIdAsync(id);

            if (attribute != null)
            {
                return this.Ok(this.mapper.Map<AttributeResponse>(attribute));
            }

            return this.NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAttributesAsync()
        {
            await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());

            List<Attribute> abilities = await this.repository.GetAllAttributesAsync();

            return this.Ok(abilities.Select(attribute => this.mapper.Map<AttributeResponse>(attribute)).ToList());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttributeAsync(Guid id)
        {
            await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());

            await this.repository.DeleteAttributeAsync(id);

            return this.Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAttributeAsync(Guid id, [FromBody] AttributeRequest request)
        {
            await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());

            Attribute attribute = this.mapper.Map<Attribute>(request);
            await this.repository.UpdateAttributeAsync(id, attribute);

            return this.Ok(this.mapper.Map<AttributeResponse>(attribute));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAttributeAsync([FromBody] AttributeRequest request)
        {
            await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());

            Attribute attribute = this.mapper.Map<Attribute>(request);
            await this.repository.CreateAttributeAsync(attribute);

            return this.Ok(this.mapper.Map<AttributeResponse>(attribute));
        }

    }
}
