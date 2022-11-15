﻿using Hero.Server.Core;
using Hero.Server.Core.Database;
using Hero.Server.Core.Repositories;

namespace Hero.Server.Extensions
{
    public static class WebApplicationExtension
    {
        public static async Task EnsureGlobalAttributesExists(this WebApplication app)
        {
            using (IServiceScope scope =  app.Services.CreateScope())
            {
                IGroupContextBuilder builder = scope.ServiceProvider.GetRequiredService<IGroupContextBuilder>();
                IAttributeRepository repository = scope.ServiceProvider.GetRequiredService<IAttributeRepository>();

                builder.Apply(new Guid());
                await repository.CreateIfNotExistsAsync(GlobalAttribute.Health);
            }
        }
    }
}
