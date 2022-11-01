using Hero.Server.Core.Logging;
using Hero.Server.Core.Models;
using Hero.Server.Core.Repositories;
using Hero.Server.DataAccess.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hero.Server.DataAccess.Repositories
{
    public class SkilltreeRepository : ISkilltreeRepository
    {
        private readonly HeroDbContext context;
        private readonly ILogger<SkilltreeRepository> logger;

        public SkilltreeRepository(HeroDbContext context, ILogger<SkilltreeRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<List<Skilltree>> GetAllSkilltreesOfCharacterAsync(Guid charId, CancellationToken cancellationToken = default)
        {
            return await this.context.Skilltrees.Where(c => c.CharacterId == charId).ToListAsync(cancellationToken);
        }

        public async Task<Skilltree?> GetSkilltreeByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await this.context.Skilltrees
                .Include(c => c.Nodes)
                .ThenInclude(n => n.Skill)
                .ThenInclude(s => s.Ability)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task CreateSkilltreeAsync(Skilltree skilltree, CancellationToken cancellationToken = default)
        {
            try
            {
                await this.context.Skilltrees.AddAsync(skilltree, cancellationToken);
                await this.context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
        }

        public async Task DeleteSkilltreeAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                Skilltree? existing = await this.context.Skilltrees.FindAsync(new object[] { id }, cancellationToken);

                if (null == existing)
                {
                    this.logger.LogSkilltreeDoesNotExist(id);
                    return;
                }

                this.context.Skilltrees.Remove(existing);
                await this.context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
        }

        public async Task UpdateSkilltreeAsync(Guid id, Skilltree updatedNodeTree, CancellationToken cancellationToken = default)
        {
            try
            {
                Skilltree? existing = await this.context.Skilltrees.FindAsync(new object[] { id }, cancellationToken);

                if (null == existing)
                {
                    throw new Exception($"The nodeTree (id: {id}) you're trying to update does not exist.");
                }

                //Todo testen: Nodes löschen/updaten/einfügen Verhalten bezüglich NodeTree und DB.
                existing.Nodes.Clear();
                existing.Update(updatedNodeTree);

                this.context.Skilltrees.Update(existing);
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
