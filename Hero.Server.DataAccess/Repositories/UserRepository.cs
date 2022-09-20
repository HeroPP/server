using Hero.Server.Core.Logging;
using Hero.Server.Core.Models;
using Hero.Server.Core.Repositories;
using Hero.Server.DataAccess.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hero.Server.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly HeroDbContext context;
        private readonly ILogger<UserRepository> logger;

        public UserRepository(HeroDbContext context, ILogger<UserRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<User> CreateUserAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                User user = new()
                {
                    Id = id,
                };

                await this.context.Users.AddAsync(user, cancellationToken);
                await this.context.SaveChangesAsync(cancellationToken);

                return user;
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
        }

        public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await this.context.Users
                .Include(u => u.OwnedGroup)
                .FirstOrDefaultAsync(item => id == item.Id, cancellationToken);
        }

        public async Task<IEnumerable<User>> GetUsersByIdAsync(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            return await this.context.Users
                .Where(item => ids.Contains(item.Id))
                .ToListAsync(cancellationToken);
        }
    }
}
