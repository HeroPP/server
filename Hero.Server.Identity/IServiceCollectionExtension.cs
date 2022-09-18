using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System.Text.Json;

namespace Hero.Server.Identity
{
    public static class IServiceCollectionExtension
    {

        public static void AddSwaggerForAuthentication(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        public static AuthenticationBuilder AddJwtBearerAuthentication(this IServiceCollection services, Action<JwtBearerOptions>? configureOptions = default)
        {
            return services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Authority = AuthenticationDefaults.Issuer;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    AuthenticationType = JwtBearerDefaults.AuthenticationScheme,
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = AuthenticationDefaults.Issuer
                };
                configureOptions?.Invoke(options);
            });
        }
    }
}
