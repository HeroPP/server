using Hero.Server.Core;
using Hero.Server.Core.Logging;
using Hero.Server.Core.Models;
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


        [HttpGet, Authorize(Roles = RoleNames.Administrator)]
        public async Task<IActionResult> GetGroupAdminInfo(CancellationToken token)
        {
            return await this.HandleExceptions(async () =>
            {
                Group? group = await this.repository.GetGroupByOwnerId(this.HttpContext.User.GetUserId(), token);
                if (null == group) 
                {
                    return this.BadRequest();
                }

                return this.Ok(new { Id = group.Id, Name = group.Name, Code = $"https://hero-app.de/invite?code={group.InviteCode}" });
            });
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetGroupInfo(string code, CancellationToken token)
        {
            return await this.HandleExceptions(async () =>
            {
                Group group = await this.repository.GetGroupByInviteCode(code, token);
                UserInfo user = await this.repository.GetGroupOwner(group, token);

                return this.Ok(new { Id = group.Id, Name = group.Name, Owner = user.Username, Description = group.Description });
            });
        }
        
        [HttpGet("users"), Authorize(Roles = RoleNames.Administrator)]
        public async Task<IActionResult> GetAllUsersInGroup(CancellationToken token)
        {
            return await this.HandleExceptions(async () =>
            {
                List<UserInfo> users = await this.repository.GetAllUsersInGroupAsync(this.HttpContext.User.GetUserId(), token);
                return this.Ok(users);
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody]GroupRequest request, CancellationToken token)
        {
            return await this.HandleExceptions(async () =>
            {
                string? code = await this.repository.CreateGroup(request.Name, request.Description, this.HttpContext.User.GetUserId(), token);
                if (null != code)
                {
                    // ToDo: Generate invitation code.
                    this.logger.LogGroupCreatedSuccessfully(request.Name);

                    return this.Ok();
                }

                return this.BadRequest(new ErrorResponse((int)EventIds.GroupCreationFailed, "The group you want to create already exists, please choose another name."));
            });
        }


        [HttpPost("{id}/join/{code}")]
        public async Task<IActionResult> JoinGroup(Guid id, string code, CancellationToken token)
        {
            return await this.HandleExceptions(async () =>
            {
                await this.repository.JoinGroup(id, this.HttpContext.User.GetUserId(), code, token);
                return this.Ok();
            });
        }

        [HttpPost("leave")]
        public async Task<IActionResult> LeaveGroup(CancellationToken token)
        {
            return await this.HandleExceptions(async () =>
            {
                await this.repository.LeaveGroup(this.HttpContext.User.GetUserId(), token);
                return this.Ok(); 
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(Guid id, CancellationToken token)
        {
            return await this.HandleExceptions(async () =>
            {
                await this.repository.DeleteGroup(id, this.HttpContext.User.GetUserId(), token);
                return this.Ok();
            });
        }
    }
}
