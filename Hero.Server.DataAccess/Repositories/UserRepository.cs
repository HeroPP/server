using Hero.Server.Core;
using Hero.Server.Core.Exceptions;
using Hero.Server.Core.Logging;
using Hero.Server.Core.Models;
using Hero.Server.Core.Repositories;
using Hero.Server.DataAccess.Database;

using JCurth.Keycloak;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hero.Server.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly HeroDbContext context;
        private readonly IGroupContext group;
        private readonly ILogger<UserRepository> logger;

        public UserRepository(HeroDbContext context, IGroupContext group, ILogger<UserRepository> logger)
        {
            this.context = context;
            this.group = group;
            this.logger = logger;
        }


        public async Task EnsureIsOwner(Guid userId, CancellationToken cancellationToken = default)
        {
            User? user = await this.GetUserByIdAsync(userId, cancellationToken);
            if (null == user?.OwnedGroup || user.OwnedGroup.Id != this.group.Id)
            {
                throw new BaseException((int)ErrorCode.NotGroupAdmin, "You are not the admin of this group.");
            }
        }

        public async Task<User> CreateUserIfNotExistAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                User user = new()
                {
                    Id = id,
                };

                if (this.context.Users.Find(id) == null)
                {
                    await this.context.Users.AddAsync(user, cancellationToken);
                    await this.context.SaveChangesAsync(cancellationToken);
                }

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
                .Include(g => g.Group)
                .IgnoreQueryFilters()
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
