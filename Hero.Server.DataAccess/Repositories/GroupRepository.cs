using Hero.Server.Core;
using Hero.Server.Core.Configuration;
using Hero.Server.Core.Exceptions;
using Hero.Server.Core.Logging;
using Hero.Server.Core.Models;
using Hero.Server.Core.Repositories;
using Hero.Server.DataAccess.Database;

using JCurth.Keycloak;
using JCurth.Keycloak.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System;

namespace Hero.Server.DataAccess.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private const string DefaultGroupName = "Users";
        private const string AdminGroupName = "Administrator";

        private readonly IKeycloakService service;
        private readonly HeroDbContext context;
        private readonly ILogger<GroupRepository> logger;
        private readonly KeycloakOptions options;
        private readonly MappingOptions mappings;

        public GroupRepository(IKeycloakService service, IOptions<KeycloakOptions> options, IOptions<MappingOptions> mappings, HeroDbContext context, ILogger<GroupRepository> logger)
        {
            this.service = service;
            this.context = context;
            this.logger = logger;
            this.options = options.Value;
            this.mappings = mappings.Value;
        }

        private string GenerateInvitationCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 12)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

        private async Task EvaluateInvitationCode(Guid groupId, string invitationCode)
        {
            Group? group = await this.context.Groups.FindAsync(groupId);
            if (group == null || String.IsNullOrEmpty(invitationCode) || !String.Equals(group.InviteCode, invitationCode, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new BaseException((int)EventIds.InvalidInvitationCode, "The given invitation code is not valid.");
            }
        }

        public async Task<Group> GetGroupByUserId(Guid userId)
        {
            Group? group = await this.context.Groups.FirstOrDefaultAsync(g => g.OwnerId == userId);
            if (null == group)
            {
                throw new BaseException((int)EventIds.NotAGroupAdmin, "You are no admin of any group, you should create one.");
            }

            return group;
        }

        public async Task<List<Core.Models.UserInfo>> GetAllUsersInGroupAsync(Guid userId)
        {
            Group? group = await this.GetGroupByUserId(userId);

            await this.service.Initialize(options);
            List<JCurth.Keycloak.Models.UserInfo> userInfos = await this.service.Groups.GetAllUsersInGroup(group.Name.ToString());

            return userInfos.Select(u => new Core.Models.UserInfo() { Id = u.Id, Email = u.Email, Firstname = u.Firstname, Lastname = u.Lastname, Username = u.Username }).ToList();
        }

        public async Task<string> CreateGroup(string groupName, string groupDescription, Guid ownerId, CancellationToken cancellationToken = default)
        {
            try
            {
                string code = this.GenerateInvitationCode();
                await this.context.Groups.AddAsync(new Group() { Name = groupName, Description = groupDescription, OwnerId = ownerId, InviteCode = code });
                await this.context.SaveChangesAsync(cancellationToken);

                await this.service.Initialize(options);

                if (this.mappings.Groups.ContainsKey("Administrator"))
                {
                    await this.service.Groups.AddUser(this.mappings.Groups["Administrator"].Id, ownerId.ToString());
                }

                return code;
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
        }

        public async Task<string> GenerateInviteCode(Guid groupId, CancellationToken cancellationToken = default)
        {
            Group? group = await this.GetGroupByUserId(groupId);

            group.InviteCode = this.GenerateInvitationCode();
            await this.context.SaveChangesAsync(cancellationToken);

            return group.InviteCode;
        }

        public async Task<bool> JoinGroup(Guid groupId, Guid userId, string invitationCode, CancellationToken cancellationToken = default)
        {
            bool success = false;
            try
            {
                await this.EvaluateInvitationCode(groupId, invitationCode);

                Group group = await this.context.Groups.FindAsync(groupId, cancellationToken);
                group!.MemberIds.Add(userId);
                await this.context.SaveChangesAsync(cancellationToken);

                await this.service.Initialize(options);
                if (this.mappings.Groups.ContainsKey("Members"))
                {
                    await this.service.Groups.AddUser(this.mappings.Groups["Members"].Id, userId.ToString());
                }
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }

            return success;
        }

        public async Task<bool> LeaveGroup(Guid userId, CancellationToken cancellationToken = default)
        {
            bool success = true;
            try
            {
                Group? group = await this.context.Groups.FirstOrDefaultAsync(g => g.MemberIds.Contains(userId));
                if (null != group)
                {
                    group.MemberIds.Remove(userId);
                    await this.context.SaveChangesAsync(cancellationToken);

                    await this.service.Initialize(options);
                    if (this.mappings.Groups.ContainsKey("Members"))
                    {
                        await this.service.Groups.RemoveUser(this.mappings.Groups["Members"].Id, userId.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }

            return success;
        }

        public async Task<bool> DeleteGroup(Guid groupId, Guid userId, CancellationToken cancellationToken = default)
        {
            bool success = false;
            try
            {
                Group? group = await this.context.Groups.FindAsync(groupId, cancellationToken);
                if (null != group && group.OwnerId == userId)
                {
                    await this.service.Initialize(options);
                    this.context.Remove(group);
                    await this.context.SaveChangesAsync(cancellationToken);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }

            return success;
        }
    }
}
