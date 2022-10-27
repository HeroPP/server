using Hero.Server.Core.Logging;
using Hero.Server.Core.Models;
using Hero.Server.Core.Repositories;
using Hero.Server.DataAccess.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Attribute = Hero.Server.Core.Models.Attribute;

namespace Hero.Server.DataAccess.Repositories
{
    public class AttributeRepository : IAttributeRepository
    {
        private readonly HeroDbContext context;
        private readonly IUserRepository userRepository;
        private readonly ILogger<AttributeRepository> logger;

        public AttributeRepository(HeroDbContext context, IUserRepository userRepository, ILogger<AttributeRepository> logger)
        {
            this.context = context;
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public async Task<Attribute?> GetAttributeByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            User? user = await this.userRepository.GetUserByIdAsync(userId, cancellationToken);
            return await this.context.Attributes.Where(a => a.GroupId == user!.OwnedGroup.Id).FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
        }

        public async Task CreateAttributeAsync(Attribute attribute, Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                User? user = await this.userRepository.GetUserByIdAsync(userId, cancellationToken);
                if (null != user)
                {
                    attribute.GroupId = user.OwnedGroup.Id;
                    await this.context.Attributes.AddAsync(attribute, cancellationToken);
                    await this.context.SaveChangesAsync(cancellationToken);
                }
                
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
        }

        public async Task DeleteAttributeAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                Attribute? existing = await this.GetAttributeByIdAsync(id, userId, cancellationToken);
                if(null == existing)
                {
                    this.logger.LogAttributeDoesNotExist(id);
                    return;
                }
                this.context.Attributes.Remove(existing);
                await this.context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
        }

        public async Task<IEnumerable<Attribute>> GetAllAttributesAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            User? user = await this.userRepository.GetUserByIdAsync(userId, cancellationToken);
            return await this.context.Attributes.Where(a => a.GroupId == user!.OwnedGroup.Id).ToListAsync(cancellationToken);
        }

        public async Task UpdateAttributeAsync(Guid id, Attribute updatedAttribute, Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                Attribute? existing = await this.GetAttributeByIdAsync(id, userId, cancellationToken);

                if (null == existing)
                {
                    throw new Exception($"The Attribute (id: {id}) you're trying to update does not exist.");
                }

                existing.Update(updatedAttribute);

                this.context.Attributes.Update(existing);
                await this.context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
        }
    }
}
