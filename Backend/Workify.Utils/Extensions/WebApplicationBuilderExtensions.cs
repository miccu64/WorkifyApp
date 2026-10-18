using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Workify.Utils.Config;

namespace Workify.Utils.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        extension(WebApplicationBuilder builder)
        {
            public T CommonApiInitialization<T>()
                where T : CommonConfig
            {
                T config = builder.InitConfigFromEnvironmentVariables<T>();

                builder.AddJwtAuth(config);

                builder.Services.AddHttpContextAccessor();

                builder.AddLogging(config);

                builder.Services.AddControllers();
                builder.Services.AddOpenApi();

                return config;
            }

            private T InitConfigFromEnvironmentVariables<T>()
                where T : CommonConfig
            {
                builder.Configuration.AddEnvironmentVariables();

                IConfigurationSection section = builder.Configuration.GetSection(CommonConfig.EnvironmentGroup);
                builder.Services.Configure<T>(section);

                return section.Get<T>()!;
            }

            private void AddJwtAuth<T>(T config)
                where T : CommonConfig
            {
                builder
                    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(x =>
                    {
                        x.RequireHttpsMetadata = false;
                        x.SaveToken = false;

                        RSA rsa = RSA.Create();
                        rsa.ImportFromPem(
                            "-----BEGIN PUBLIC KEY-----\n"
                            + config.BearerPublicKey
                            + "\n-----END PUBLIC KEY-----"
                        );

                        x.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new RsaSecurityKey(rsa),
                            ValidateIssuer = true,
                            ValidIssuer = CommonConfig.JwtIssuer,
                            ValidateAudience = false,
                            ClockSkew = TimeSpan.Zero,
                        };
                    });

                builder.Services.AddAuthorization(options =>
                {
                    options.FallbackPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                });
            }

            private void AddLogging<T>(T config)
                where T : CommonConfig
            {
                builder.Host.UseSerilog((_, _, loggerConfiguration) =>
                {
                    loggerConfiguration
                        .Enrich.WithCorrelationIdHeader()
                        .Enrich.FromLogContext()
                        .WriteTo.Seq(config.SeqConnectionString)
                        .WriteTo.Console()
                        .MinimumLevel.Information();
                });
            }
        }
    }
}