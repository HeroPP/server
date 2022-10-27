using AutoMapper;
using Hero.Server.Core.Models;
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
        private readonly IMapper mapper;

        public AttributesController(IAttributeRepository repository, IMapper mapper, ILogger<AttributesController> logger) 
            : base(logger)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet("{name}")]
        public Task<IActionResult> GetAttributeByIdAsync(Guid id)
        {
            return this.HandleExceptions(async () =>
            {
                Attribute? attribute = await this.repository.GetAttributeByIdAsync(id, this.HttpContext.User.GetUserId());
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
                List<Attribute> abilities = (await this.repository.GetAllAttributesAsync(this.HttpContext.User.GetUserId())).ToList();

                return this.Ok(abilities.Select(attribute => this.mapper.Map<AttributeResponse>(attribute)).ToList());
            });
        }

        [HttpDelete("{name}")]
        public Task<IActionResult> DeleteAttributeAsync(Guid id)
        {
            return this.HandleExceptions(async () =>
            {
                await this.repository.DeleteAttributeAsync(id, this.HttpContext.User.GetUserId());
                return this.Ok();
            });
        }

        [HttpPut("{name}")]
        public Task<IActionResult> UpdateAttributeAsync(Guid id, [FromBody] CreateAttributeRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Attribute attribute = this.mapper.Map<Attribute>(request);
                await this.repository.UpdateAttributeAsync(id, attribute, this.HttpContext.User.GetUserId());
                return this.Ok(this.mapper.Map<AttributeResponse>(attribute));
            });
        }

        [HttpPost]
        public Task<IActionResult> CreateAttributeAsync([FromBody] CreateAttributeRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Attribute attribute = this.mapper.Map<Attribute>(request);
                await this.repository.CreateAttributeAsync(attribute, this.HttpContext.User.GetUserId());

                return this.Ok(this.mapper.Map<AttributeResponse>(attribute));
            });
        }

    }
}
