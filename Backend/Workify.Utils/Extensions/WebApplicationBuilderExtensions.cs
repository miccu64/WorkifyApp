using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using Workify.Utils.Config;

namespace Workify.Utils.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static T CommonApiInitialization<T>(this WebApplicationBuilder builder)
            where T : CommonConfig
        {
            T config = builder.InitConfigFromEnvironmentVariables<T>();

            builder.AddJwtAuth(config);

            builder.Services.AddControllers();

            return config;
        }

        private static T InitConfigFromEnvironmentVariables<T>(this WebApplicationBuilder builder)
            where T : CommonConfig
        {
            builder.Configuration.AddEnvironmentVariables();

            IConfigurationSection section = builder.Configuration.GetSection(CommonConfig.EnvironmentGroup);
            builder.Services.Configure<T>(section);

            return section.Get<T>()!;
        }

        private static void AddJwtAuth<T>(this WebApplicationBuilder builder, T config)
            where T : CommonConfig
        {
            builder
                .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = false;

                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.BearerKey)),
                        ValidateIssuer = true,
                        ValidIssuer = CommonConfig.JwtIssuer,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero,
                    };

                    x.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"JWT Auth failed: {context.Exception.Message}");
                            return Task.CompletedTask;
                        }
                    };
                });
            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });
        }
    }
}
