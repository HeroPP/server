﻿using Hero.Server.Core;
using Hero.Server.Core.Logging;
using Hero.Server.Core.Repositories;
using Hero.Server.Identity;
using Hero.Server.Messages.Requests;
using Hero.Server.Messages.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hero.Server.Controllers
{
    [ApiController, Authorize, Route("api/[controller]")]
    public class GroupsController : HeroControllerBase
    {
        private readonly IGroupRepository repository;

        public GroupsController(IGroupRepository repository, ILogger<GroupsController> logger)
            : base(logger)
        {
            this.repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody]GroupRequest request)
        {
            return await this.HandleExceptions(async () =>
            {

                string? code = await this.repository.CreateGroup(request.Name, this.HttpContext.User.GetUserId());
                if (null != code)
                {
                    // ToDo: Generate invitation code.
                    this.logger.LogGroupCreatedSuccessfully(request.Name);

                    return this.Ok(new { InvitationCode = code});
                }

                return this.BadRequest(new ErrorResponse((int)EventIds.GroupCreationFailed, "The group you want to create already exists, please choose another name."));
            });
        }

        [HttpPost("{id}/join/{code}")]
        public async Task<IActionResult> JoinGroup(Guid id, string code)
        {
            return await this.HandleExceptions(async () =>
            {
                return await this.repository.JoinGroup(id, this.HttpContext.User.GetUserId(), code) ? this.Ok() : this.BadRequest();
            });
        }

        [HttpPost("leave")]
        public async Task<IActionResult> LeaveGroup()
        {
            return await this.HandleExceptions(async () =>
            {
                return await this.repository.LeaveGroup(this.HttpContext.User.GetUserId()) 
                ? this.Ok() 
                : this.BadRequest();
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(Guid id)
        {
            return await this.HandleExceptions(async () =>
            {
                return await this.repository.DeleteGroup(id, this.HttpContext.User.GetUserId()) 
                ? this.Ok() 
                : this.BadRequest();
            });
        }
    }
}