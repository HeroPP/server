using AutoMapper;
using Hero.Server.Core.Models;
using Hero.Server.Core.Repositories;
using Hero.Server.Identity;
using Hero.Server.Messages.Requests;
using Hero.Server.Messages.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Attribute = Hero.Server.Core.Models.Attribute;

namespace Hero.Server.Controllers
{
    [ApiController, Authorize(Roles = RoleNames.Administrator), Route("api/[controller]")]
    public class SkillsController : HeroControllerBase
    {
        private readonly ISkillRepository repository;
        private readonly IMapper mapper;

        public SkillsController(ISkillRepository repository, IMapper mapper, ILogger<SkillsController> logger)
            : base(logger)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public Task<IActionResult> GetSkillByIdAsync(Guid id)
        {
            return this.HandleExceptions(async () =>
            {
                Skill? skill = await this.repository.GetSkillByIdAsync(id, this.HttpContext.User.GetUserId());
                if (skill != null)
                {
                    return this.Ok(new SkillResponse(skill, this.mapper));
                }

                return this.BadRequest();
            });
        }

        [HttpGet]
        public Task<IActionResult> GetAllSkillsAsync()
        {
            return this.HandleExceptions(async () =>
            {
                List<Skill> skills = (await this.repository.GetAllSkillsAsync(this.HttpContext.User.GetUserId())).ToList();

                return this.Ok(skills.Select(skill => new SkillResponse(skill, this.mapper)).ToList());
            });
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteSkillAsync(Guid id)
        {
            return this.HandleExceptions(async () =>
            {
                await this.repository.DeleteSkillAsync(id, this.HttpContext.User.GetUserId());
                return this.Ok();
            });
        }

        [HttpPut("{id}")]
        public Task<IActionResult> UpdateSkillAsync(Guid id, [FromBody] CreateSkillRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Skill skill = new Skill
                {
                    Id = id,
                    Name = request.Name,
                    Description = request.Description,
                    AbilityName = request.AbilityName,
                    IconUrl = request.IconUrl,
                    Ability = this.mapper.Map<Ability>(request.CreateAbilityRequest),
                    //Ich kenne mich mit Tasks nicht genug aus ._.
                    AttributeSkills = request.UpdateAttributeValueRequests.Select(async uavr => this.repository.GetAttributeSkillByIdAsync(id, uavr.AttributeId, this.HttpContext.User.GetUserId()) == null ? 
                    new AttributeSkill
                    {
                        AttributeId = uavr.AttributeId,
                        SkillId = id,
                        Value = uavr.Value,
                        Attribute = this.mapper.Map<Attribute>(uavr.CreateAttributeRequest),
                    }
                    :
                    await this.repository.GetAttributeSkillByIdAsync(id, uavr.AttributeId, this.HttpContext.User.GetUserId())
                    ).ToList(),
                };
                skill.AttributeSkills.ForEach(ats => ats.Skill = skill);
                await this.repository.UpdateSkillAsync(id, skill, this.HttpContext.User.GetUserId());
                return this.Ok(new SkillResponse(skill, this.mapper));
            });
        }

        [HttpPost]
        public Task<IActionResult> CreateSkillAsync([FromBody] CreateSkillRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                Guid id = Guid.NewGuid();
                Skill skill = new Skill
                {
                    Id = id,
                    Name = request.Name,
                    Description = request.Description,
                    AbilityName = request.AbilityName,
                    IconUrl = request.IconUrl,
                    Ability = this.mapper.Map<Ability>(request.CreateAbilityRequest),
                    AttributeSkills = request.UpdateAttributeValueRequests.Select(uavr => new AttributeSkill
                    {
                        AttributeId = uavr.AttributeId,
                        SkillId = id,
                        Value = uavr.Value,
                        Attribute = this.mapper.Map<Attribute>(uavr.CreateAttributeRequest),
                    }).ToList(),

                };
                skill.AttributeSkills.ForEach(ats => ats.Skill = skill);
                await this.repository.CreateSkillAsync(skill, this.HttpContext.User.GetUserId());

                return this.Ok(new SkillResponse(skill, this.mapper));
            });
        }

    }
}
