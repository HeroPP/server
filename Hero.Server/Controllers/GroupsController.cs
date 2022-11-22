﻿using Hero.Server.Core.Logging;
using Hero.Server.Core.Models;
using Hero.Server.Core.Repositories;
using Hero.Server.Identity;
using Hero.Server.Messages.Requests;

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

        [Route("/error"), ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HandleError() => this.HandleErrors();

        [HttpGet, Authorize(Roles = RoleNames.Administrator)]
        public async Task<IActionResult> GetGroupAdminInfo(CancellationToken token)
        {
            Group group = await this.repository.GetGroupByOwnerId(this.HttpContext.User.GetUserId(), token);

            return this.Ok(new { Id = group.Id, Name = group.Name, Code = $"https://hero-app.de/invite?code={group.InviteCode}" });
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetGroupInfo(string code, CancellationToken token)
        {
            Group group = await this.repository.GetGroupByInviteCode(code, token);
            UserInfo user = await this.repository.GetGroupOwner(group, token);

            return this.Ok(new { Id = group.Id, Name = group.Name, Owner = user.Username, Description = group.Description });
        }
        
        [HttpGet("users"), Authorize(Roles = RoleNames.Administrator)]
        public async Task<IActionResult> GetAllUsersInGroup(CancellationToken token)
        {
            List<UserInfo> users = await this.repository.GetAllUsersInGroupAsync(this.HttpContext.User.GetUserId(), token);

            return this.Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody]GroupRequest request, CancellationToken token)
        {
                string code = await this.repository.CreateGroup(request.Name, request.Description, this.HttpContext.User.GetUserId(), token);
                this.logger.LogGroupCreatedSuccessfully(request.Name);

                return this.Ok();
        }


        [HttpPost("{id}/join/{code}")]
        public async Task<IActionResult> JoinGroup(Guid id, string code, CancellationToken token)
        {
            await this.repository.JoinGroup(id, this.HttpContext.User.GetUserId(), code, token);

            return this.Ok();
        }

        [HttpPost("leave")]
        public async Task<IActionResult> LeaveGroup(CancellationToken token)
        {
            await this.repository.LeaveGroup(this.HttpContext.User.GetUserId(), token);

            return this.Ok(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(Guid id, CancellationToken token)
        {
            await this.repository.DeleteGroup(id, this.HttpContext.User.GetUserId(), token);

            return this.Ok();
        }
    }
}
