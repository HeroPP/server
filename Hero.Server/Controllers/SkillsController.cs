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
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public SkillsController(ISkillRepository repository, IUserRepository userRepository, IMapper mapper, ILogger<SkillsController> logger)
            : base(logger)
        {
            this.repository = repository;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public Task<IActionResult> GetSkillByIdAsync(Guid id, CancellationToken token)
        {
            return this.HandleExceptions(async () =>
            {
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                Skill? skill = await this.repository.GetSkillByIdAsync(id, token);
                if (skill != null)
                {
                    return this.Ok(new SkillResponse(skill, this.mapper));
                }

                return this.BadRequest();
            });
        }

        [HttpGet]
        public Task<IActionResult> GetAllSkillsAsync(CancellationToken token)
        {
            return this.HandleExceptions(async () =>
            {
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                List<Skill> skills = (await this.repository.GetAllSkillsAsync(token)).ToList();

                return this.Ok(skills.Select(skill => new SkillResponse(skill, this.mapper)).ToList());
            });
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteSkillAsync(Guid id, CancellationToken token)
        {
            return this.HandleExceptions(async () =>
            {
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                await this.repository.DeleteSkillAsync(id, token);
                return this.Ok();
            });
        }

        [HttpPut("{id}")]
        public Task<IActionResult> UpdateSkillAsync(Guid id, [FromBody] CreateSkillRequest request, CancellationToken token)
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
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                await this.repository.UpdateSkillAsync(id, skill, token);
                return this.Ok(new SkillResponse(skill, this.mapper));
            });
        }

        [HttpPost]
        public Task<IActionResult> CreateSkillAsync([FromBody] CreateSkillRequest request, CancellationToken token)
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
                await userRepository.EnsureIsOwner(this.HttpContext.User.GetUserId());
                await this.repository.CreateSkillAsync(skill, token);

                return this.Ok(new SkillResponse(skill, this.mapper));
            });
        }

    }
}
