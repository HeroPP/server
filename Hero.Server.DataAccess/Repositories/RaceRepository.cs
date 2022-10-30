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
        private readonly ILogger<RaceRepository> logger;

        public RaceRepository(HeroDbContext context, IUserRepository userRepository, ILogger<RaceRepository> logger)
        {
            this.context = context;
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public async Task<Race?> GetRaceByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            User? user = await this.userRepository.GetUserByIdAsync(userId, cancellationToken);
            return await this.context.Races.Where(a => a.GroupId == user!.OwnedGroup.Id).FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
        }

        public async Task CreateRaceAsync(Race race, Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                User? user = await this.userRepository.GetUserByIdAsync(userId, cancellationToken);
                if (null != user)
                {
                    race.GroupId = user.OwnedGroup.Id;
                    await this.context.Races.AddAsync(race, cancellationToken);
                    await this.context.SaveChangesAsync(cancellationToken);
                }
                
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                throw;
            }
        }

        public async Task DeleteRaceAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                Race? existing = await this.GetRaceByIdAsync(id, userId, cancellationToken);
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

        public async Task<IEnumerable<Race>> GetAllRacesAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            User? user = await this.userRepository.GetUserByIdAsync(userId, cancellationToken);
            return await this.context.Races.Where(a => a.GroupId == user!.OwnedGroup.Id).ToListAsync(cancellationToken);
        }

        public async Task UpdateRaceAsync(Guid id, Race updatedRace, Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                Race? existing = await this.GetRaceByIdAsync(id, userId, cancellationToken);

                if (null == existing)
                {
                    throw new Exception($"The Race (id: {id}) you're trying to update does not exist.");
                }

                existing.Update(updatedRace);

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
