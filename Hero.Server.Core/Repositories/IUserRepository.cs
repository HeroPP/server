using Hero.Server.Core.Models;

namespace Hero.Server.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateUserIfNotExistAsync(string id, CancellationToken cancellationToken = default);
        Task EnsureIsOwner(string userId, CancellationToken cancellationToken = default);
        Task<User?> GetUserByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<List<User>> GetUsersByIdAsync(List<string> ids, CancellationToken cancellationToken = default);
    }
}
