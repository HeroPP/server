using Hero.Server.Core.Models;

namespace Hero.Server.Core.Repositories
{
    public interface IGroupRepository
    {
        Task<string> CreateGroup(string groupName, string? groupDescription, Guid ownerId, CancellationToken cancellationToken = default);
        Task<string> GenerateInviteCode(Guid groupId, CancellationToken cancellationToken = default);
        Task JoinGroup(Guid groupId, Guid userId, string invitationCode, CancellationToken cancellationToken = default);
        Task LeaveGroup(Guid userId, CancellationToken cancellationToken = default);
        Task DeleteGroup(Guid groupId, Guid userId, CancellationToken cancellationToken = default);
        Task<Group> GetGroupByOwnerId(Guid userId, CancellationToken cancellationToken = default);
        Task<List<UserInfo>> GetAllUsersInGroupAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Group?> GetGroupByUserId(Guid userId, CancellationToken cancellationToken = default);
        Task<Group> GetGroupByInviteCode(string invitationCode, CancellationToken cancellationToken = default);
        Task<UserInfo> GetGroupOwner(Group group, CancellationToken cancellationToken = default);
    }
}
