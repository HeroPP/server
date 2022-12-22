using Hero.Server.Core.Configuration;
using Hero.Server.DataAccess.Extensions;
using Hero.Server.Extensions;
using Hero.Server.Identity;

using JCurth.Keycloak;

using Microsoft.AspNetCore.Diagnostics;

using static System.Net.Mime.MediaTypeNames;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KeycloakOptions>(options => builder.Configuration.GetSection("Services:Keycloak").Bind(options));
builder.Services.Configure<MappingOptions>(options => builder.Configuration.GetSection("Services:Keycloak").Bind(options));

builder.Services.AddJwtBearerAuthentication();
builder.Services.AddDataAccessLayer(builder.Configuration.GetConnectionString("Default"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSwaggerForAuthentication();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options => {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    options.RoutePrefix = String.Empty; 
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.MigrateDatabaseAsync();
await app.EnsureGlobalAttributesExists();

app.UseExceptionHandler("/error");

app.Run();
