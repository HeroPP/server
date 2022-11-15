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
        private readonly IGroupContext group;
        private readonly IUserRepository userRepository;
        private readonly ILogger<AttributeRepository> logger;

        public AttributeRepository(HeroDbContext context, IGroupContext group, IUserRepository userRepository, ILogger<AttributeRepository> logger)
        {
            this.context = context;
            this.group = group;
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public async Task<Attribute?> GetAttributeByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await this.context.Attributes.FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
        }

        // This method should only be used for global attributes and never should be used inside a controller, because it ignores every group constraints.
        public async Task CreateIfNotExistsAsync(Attribute attribute, CancellationToken cancellationToken = default)
        {
            try
            {
                if (null == await this.GetAttributeByIdAsync(attribute.Id, cancellationToken))
                {
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

        public async Task CreateAttributeAsync(Attribute attribute, CancellationToken cancellationToken = default)
        {
            try
            {
                    attribute.GroupId = this.group.Id;
                    await this.context.Attributes.AddAsync(attribute, cancellationToken);
                    await this.context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
        }

        public async Task DeleteAttributeAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                Attribute? existing = await this.GetAttributeByIdAsync(id, cancellationToken);
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

        public async Task<IEnumerable<Attribute>> GetAllAttributesAsync(CancellationToken cancellationToken = default)
        {
            return await this.context.Attributes.ToListAsync(cancellationToken);
        }

        public async Task UpdateAttributeAsync(Guid id, Attribute updatedAttribute, CancellationToken cancellationToken = default)
        {
            try
            {
                Attribute? existing = await this.GetAttributeByIdAsync(id, cancellationToken);

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
