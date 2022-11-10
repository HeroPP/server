using Hero.Server.Core.Logging;
using Hero.Server.Core.Models;
using Hero.Server.Core.Repositories;
using Hero.Server.DataAccess.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hero.Server.DataAccess.Repositories
{
    public class RaceRepository : IRaceRepository
    {
        private readonly HeroDbContext context;
        private readonly IUserRepository userRepository;
        private readonly IGroupContext group;
        private readonly ILogger<RaceRepository> logger;

        public RaceRepository(HeroDbContext context, IUserRepository userRepository, IGroupContext group, ILogger<RaceRepository> logger)
        {
            this.context = context;
            this.userRepository = userRepository;
            this.group = group;
            this.logger = logger;
        }

        public async Task<Race?> GetRaceByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await this.context.Races.Include(r => r.AttributeRaces).ThenInclude(ar => ar.Attribute).FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
        }

        public async Task CreateRaceAsync(Race race, CancellationToken cancellationToken = default)
        {
            try
            {
                race.GroupId = group.Id;
                race.Id = Guid.NewGuid();
                race.AttributeRaces.ForEach(ats => ats.RaceId = race.Id);

                await this.context.Races.AddAsync(race, cancellationToken);
                await this.context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
        }

        public async Task DeleteRaceAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                Race? existing = await this.GetRaceByIdAsync(id, cancellationToken);
                if(null == existing)
                {
                    this.logger.LogRaceDoesNotExist(id);
                    return;
                }
                this.context.Races.Remove(existing);
                await this.context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
        }

        public async Task<IEnumerable<Race>> GetAllRacesAsync(CancellationToken cancellationToken = default)
        {
            return await this.context.Races.Include(r => r.AttributeRaces).ThenInclude(ar => ar.Attribute).ToListAsync(cancellationToken);
        }

        public async Task UpdateRaceAsync(Guid id, Race updatedRace, CancellationToken cancellationToken = default)
        {
            try
            {
                Race? existing = await this.GetRaceByIdAsync(id, cancellationToken);

                if (null == existing)
                {
                    throw new Exception($"The Race (id: {id}) you're trying to update does not exist.");
                }

                existing.Update(updatedRace);

                foreach (AttributeRace existingAttributeRace in existing.AttributeRaces.Where(ats => updatedRace.AttributeRaces.Select(x => (x.RaceId, x.AttributeId)).Contains((ats.RaceId, ats.AttributeId))))
                {
                    AttributeRace updatedAttributeRace = updatedRace.AttributeRaces.Single(ats => existingAttributeRace.RaceId == ats.RaceId && existingAttributeRace.AttributeId == ats.AttributeId);
                    existingAttributeRace.Value = updatedAttributeRace.Value;
                }

                existing.AttributeRaces.RemoveAll(ats => !updatedRace.AttributeRaces.Select(x => (x.RaceId, x.AttributeId)).Contains((ats.RaceId, ats.AttributeId)));
                existing.AttributeRaces.AddRange(updatedRace.AttributeRaces.Where(ats => !existing.AttributeRaces.Select(x => (x.RaceId, x.AttributeId)).Contains((ats.RaceId, ats.AttributeId))));

                this.context.Races.Update(existing);
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
