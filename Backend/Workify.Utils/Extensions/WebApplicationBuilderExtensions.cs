using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Workify.Utils.Config;

namespace Workify.Utils.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder CommonApiInitialization<T>(this WebApplicationBuilder builder)
            where T : CommonConfig
        {
            return builder.InitCommonConfigFromEnvironmentVariables<T>().AddJwtAuth<T>();
        }

        private static WebApplicationBuilder InitCommonConfigFromEnvironmentVariables<T>(
            this WebApplicationBuilder builder
        )
            where T : CommonConfig
        {
            builder.Configuration.AddEnvironmentVariables();

            builder.Services.Configure<T>(builder.Configuration.GetSection(CommonConfig.EnvironmentGroup));

            return builder;
        }

        private static WebApplicationBuilder AddJwtAuth<T>(this WebApplicationBuilder builder)
            where T : CommonConfig
        {
            CommonConfig config = builder.Configuration.GetSection(CommonConfig.EnvironmentGroup).Get<T>()!;

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
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero,
                    };
                });

            return builder;
        }
    }
}
