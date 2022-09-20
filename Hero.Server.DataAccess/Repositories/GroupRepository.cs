using Hero.Server.Core;
using Hero.Server.Core.Configuration;
using Hero.Server.Core.Exceptions;
using Hero.Server.Core.Logging;
using Hero.Server.Core.Models;
using Hero.Server.Core.Repositories;
using Hero.Server.DataAccess.Database;

using JCurth.Keycloak;
using JCurth.Keycloak.Models;

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
        private readonly RoleMappingOptions roleMappings;

        public GroupRepository(IKeycloakService service, IOptions<KeycloakOptions> options, IOptions<RoleMappingOptions> roleMappings, HeroDbContext context, ILogger<GroupRepository> logger)
        {
            this.service = service;
            this.context = context;
            this.logger = logger;
            this.options = options.Value;
            this.roleMappings = roleMappings.Value;
        }

        private RoleConfiguration[] GetRolesForGroup(string groupName)
        {
            RoleConfiguration[] roles = new RoleConfiguration[0];
            if (this.roleMappings.RoleMapping.ContainsKey(groupName))
            {
                Role role = this.roleMappings.RoleMapping[groupName];
                roles = new RoleConfiguration[] { new() { Id = role.Id, Name = role.Name } };
            }

            return roles;
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

        public async Task<string?> CreateGroup(string groupName, Guid ownerId, CancellationToken cancellationToken = default)
        {
            string? code = null;
            try
            {
                await this.service.Initialize(options);
                if (await this.service.Groups.Create(new() { Name = groupName }))
                {
                    GroupInfo? groupInfo = await this.service.Groups.GetGroup(groupName);
                    if (null != groupInfo)
                    {
                        await this.service.Groups.AddRoles(groupInfo.Id, this.GetRolesForGroup(DefaultGroupName));
                        GroupInfo? adminGroupInfo = await this.service.Groups.AddSubGroup(groupInfo.Id, new() { Name = AdminGroupName });
                        if (null != adminGroupInfo 
                            && await this.service.Groups.AddRoles(groupInfo.Id, this.GetRolesForGroup(AdminGroupName))
                            && await this.service.Groups.AddUser(adminGroupInfo.Id, ownerId.ToString()))
                        {
                            string invitationCode = this.GenerateInvitationCode();

                            await this.context.Groups.AddAsync(new Group() { Id = Guid.Parse(groupInfo.Id), Name = groupInfo.Name, OwnerId = ownerId, InviteCode = invitationCode });
                            await this.context.SaveChangesAsync();

                            code = invitationCode;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }

            return code;
        }

        public async Task<string?> GenerateInviteCode(string groupId, CancellationToken cancellationToken = default)
        {
            Group? group = await this.context.Groups.FindAsync(groupId);
            if (null != group)
            {
                group.InviteCode = this.GenerateInvitationCode();
                await this.context.SaveChangesAsync();

                return group.InviteCode;
            }
            return null;
        }

        public async Task<bool> JoinGroup(Guid groupId, Guid userId, string invitationCode, CancellationToken cancellationToken = default)
        {
            bool success = false;
            try
            {
                await this.EvaluateInvitationCode(groupId, invitationCode);

                await this.service.Initialize(options);
                if (await this.service.Groups.AddUser(groupId.ToString(), userId.ToString()))
                {
                    User? user = await this.context.Users.FindAsync(userId);
                    if (null != user )
                    {
                        user.GroupId = groupId;
                        success = true;
                    }

                    await this.context.SaveChangesAsync();
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
                User? user = await this.context.Users.FindAsync(userId);
                if (null != user && null != user.GroupId)
                {
                    Guid groupId = user.GroupId.Value;
                    user.GroupId = null;

                    await this.service.Initialize(this.options);
                    if (!await this.service.Groups.RemoveUser(groupId.ToString(), userId.ToString()))
                    {
                        user.GroupId = groupId;
                        success = false;
                    }

                    await this.context.SaveChangesAsync();
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
                Group? group = await this.context.Groups.FindAsync(groupId);
                if (null != group && group.OwnerId == userId)
                {
                    await this.service.Initialize(options);
                    if (await this.service.Groups.Delete(groupId.ToString()))
                    {
                        this.context.Remove(group);
                        await this.context.SaveChangesAsync();
                        success = true;
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
    }
}
