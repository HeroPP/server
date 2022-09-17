﻿using Hero.Server.Core.Repositories;
using Hero.Server.DataAccess.Database;
using Hero.Server.DataAccess.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hero.Server.DataAccess.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<HeroDbContext>(
                options => options.UseNpgsql(connectionString, pgOptions => pgOptions.MigrationsHistoryTable(HeroDbResources.MigrationsTable, HeroDbResources.Schema)));

            // ToDo: Add Repositiories
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ICharacterRepository, CharacterRepository>();

            return services;
        }
    }
}
