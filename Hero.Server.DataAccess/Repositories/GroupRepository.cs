﻿using Hero.Server.Core;
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
        private readonly IKeycloakService service;
        private readonly IUserRepository userRepository;
        private readonly HeroDbContext context;
        private readonly ILogger<GroupRepository> logger;
        private readonly KeycloakOptions options;
        private readonly MappingOptions mappings;

        public GroupRepository(IKeycloakService service, IUserRepository userRepository, IOptions<KeycloakOptions> options, IOptions<MappingOptions> mappings, HeroDbContext context, ILogger<GroupRepository> logger)
        {
            this.service = service;
            this.userRepository = userRepository;
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

        private async Task EvaluateInvitationCode(Guid groupId, string invitationCode, CancellationToken cancellationToken = default)
        {
            Group? group = await this.context.Groups.SingleOrDefaultAsync(group =>  groupId == group.Id, cancellationToken);
            if (group == null || String.IsNullOrEmpty(invitationCode) || !String.Equals(group.InviteCode, invitationCode, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new GroupAccessForbiddenException("The provided invite code is invalid");
            }
        }

        public void EnsureIsMemberOrOwner(string userId)
        {
            Group? group = this.context.Groups.FirstOrDefault(g => g.OwnerId == userId || g.Members.Any(u => u.Id == userId));

            if (null == group)
            {
                throw new GroupAccessForbiddenException("You are no member of this group");
            }
        }

        public void EnsureIsOwner(string userId)
        {
            Group? group = this.context.Groups.FirstOrDefault(g => g.OwnerId == userId);

            if (null == group)
            {
                throw new GroupAccessForbiddenException("You are no admin of this group");
            }
        }

        public async Task<Group?> GetGroupByUserId(string userId, CancellationToken cancellationToken = default)
        {
            try
            {
                User? user = await this.userRepository.GetUserByIdAsync(userId, cancellationToken);
                return user?.OwnedGroup ?? user?.Group;
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw new HeroException("An error occured while getting group information.");
            }
        }

        public async Task<Group> GetGroupByOwnerId(string userId, CancellationToken cancellationToken = default)
        {
            try
            {
                User? user = await this.context.Users.Include(u => u.OwnedGroup).SingleOrDefaultAsync(u => u.Id == userId);

                if (null == user?.OwnedGroup)
                {
                    throw new GroupAccessForbiddenException("You are no admin of any group, you should create one.");
                }

                return user.OwnedGroup;
            }
            catch (HeroException ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw new HeroException("An error occured while getting group information.");
            }
        }

        //public async Task<Core.Models.UserInfo> GetGroupOwner(Group group, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        await this.service.Initialize(this.options);
        //        JCurth.Keycloak.Models.UserInfo? userInfo = await this.service.Users.GetUserById(group.OwnerId);

        //        if (null == userInfo)
        //        {
        //            throw new ObjectNotFoundException($"The group owner of group '{group.Name}' could not be determined.");
        //        }

        //        return new Core.Models.UserInfo() { Id = userInfo.Id, Email = userInfo.Email, Firstname = userInfo.Firstname, Lastname = userInfo.Lastname, Username = userInfo.Username };
        //    }
        //    catch (HeroException ex)
        //    {
        //        this.logger.LogUnknownErrorOccured(ex);
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        this.logger.LogUnknownErrorOccured(ex);
        //        throw new HeroException("An error occured while getting group owner.");
        //    }
        //}

        public async Task<Group> GetGroupByInviteCode(string invitationCode, CancellationToken cancellationToken = default)
        {
            try
            {
                Group? group = await this.context.Groups
                    .Include(group => group.Owner)
                    .SingleOrDefaultAsync(group => EF.Functions.ILike(group.InviteCode, invitationCode), cancellationToken);

                if (null == group)
                {
                    throw new GroupAccessForbiddenException("The provided invite code is invalid");
                }

                return group;
            }
            catch (HeroException ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw new HeroException("An error occured while getting group information.");
            }
        }

        //public async Task<List<Core.Models.UserInfo>> GetAllUsersInGroupAsync(string userId, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        Group? group = await this.context.Groups
        //            .Include(group => group.Members)
        //            .Where(group => group.OwnerId == userId).SingleOrDefaultAsync(cancellationToken);

        //        if (null == group)
        //        {
        //            throw new GroupAccessForbiddenException("You are no admin of any group, you should create one.");
        //        }

        //        await this.service.Initialize(options);
        //        List<JCurth.Keycloak.Models.UserInfo> userInfos = await this.service.Users.GetUsersById(group!.Members.Select(u => u.Id).ToList());

        //        return userInfos.Select(u => new Core.Models.UserInfo() { Id = u.Id, Email = u.Email, Firstname = u.Firstname, Lastname = u.Lastname, Username = u.Username }).ToList();
        //    }
        //    catch (HeroException ex)
        //    {
        //        this.logger.LogUnknownErrorOccured(ex);
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        this.logger.LogUnknownErrorOccured(ex);
        //        throw new HeroException("An error occured while getting all users in group.");
        //    }
        //}

        public async Task<string> CreateGroup(string groupName, string? groupDescription, string ownerId, CancellationToken cancellationToken = default)
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
                throw new HeroException("An error occured while creating the group.");
            }
        }

        public async Task JoinGroup(Guid groupId, string userId, string invitationCode, CancellationToken cancellationToken = default)
        {
            try
            {
                await this.EvaluateInvitationCode(groupId, invitationCode, cancellationToken);

                await this.service.Initialize(options);
                if (this.mappings.Groups.ContainsKey("Member"))
                {
                    await this.service.Groups.AddUser(this.mappings.Groups["Member"].Id, userId.ToString());
                }
                else
                {
                    throw new HeroException("Groupmapping is not setup, could not join members group.");
                }

                User? user = await this.userRepository.GetUserByIdAsync(userId);

                if (null != user)
                {
                    user.GroupId = groupId;
                }

                await this.context.SaveChangesAsync(cancellationToken);
            }
            catch (HeroException ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw new HeroException("An error occured while joining the group.");
            }
        }

        public async Task LeaveGroup(string userId, CancellationToken cancellationToken = default)
        {
            try
            {
                User? user = await this.userRepository.GetUserByIdAsync(userId);
                if (null != user)
                {
                    await this.service.Initialize(options);
                    if (this.mappings.Groups.ContainsKey("Members"))
                    {
                        await this.service.Groups.RemoveUser(this.mappings.Groups["Members"].Id, userId.ToString());
                    }
                    else
                    {
                        throw new HeroException("Groupmapping is not setup, could not leave members group.");
                    }

                    user.GroupId = null;

                    await this.context.SaveChangesAsync(cancellationToken);

                    
                }
            }
            catch (HeroException ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw new HeroException("An error occured while leaving the group.");
            }
        }

        public async Task DeleteGroup(Guid groupId, string userId, CancellationToken cancellationToken = default)
        {
            try
            {
                Group? group = await this.context.Groups.FindAsync(groupId, cancellationToken);
                if (null != group && group.OwnerId == userId)
                {
                    await this.service.Initialize(options);
                    this.context.Remove(group);
                    await this.context.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    throw new HeroException("The group you are searching for could not be found.");
                }
            }
            catch (HeroException ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw new HeroException("An error occured while deleting the group.");
            }
        }
    }
}
