using Hero.Server.DataAccess.Extensions;
using Hero.Server.Identity;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtBearerAuthentication();
builder.Services.AddDataAccessLayer(builder.Configuration.GetConnectionString("Default"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSwaggerForAuthentication();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.MigrateDatabaseAsync();

app.Run();
