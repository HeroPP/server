namespace Hero.Server.Core.Repositories
{
    public interface IGroupRepository
    {
        Task<string?> CreateGroup(string groupName, Guid ownerId, CancellationToken cancellationToken = default);
        Task<string?> GenerateInviteCode(string groupId, CancellationToken cancellationToken = default);
        Task<bool> JoinGroup(Guid groupId, Guid userId, string invitationCode, CancellationToken cancellationToken = default);
        Task<bool> LeaveGroup(Guid userId, CancellationToken cancellationToken = default);
        Task<bool> DeleteGroup(Guid groupId, Guid userId, CancellationToken cancellationToken = default);
    }
}
