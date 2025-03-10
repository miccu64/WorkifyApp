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
        public static WebApplicationBuilder CommonApiInitialization(this WebApplicationBuilder builder)
        {
            return builder.InitCommonConfigFromEnvironmentVariables()
                .AddJwtAuth();
        }

        private static WebApplicationBuilder InitCommonConfigFromEnvironmentVariables(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddEnvironmentVariables();

            builder.Services.Configure<CommonConfig>(builder.Configuration.GetSection(CommonConfig.EnvironmentGroup));

            return builder;
        }

        private static WebApplicationBuilder AddJwtAuth(this WebApplicationBuilder builder)
        {
            CommonConfig config = builder.Configuration.GetSection(CommonConfig.EnvironmentGroup)
                    .Get<CommonConfig>()!;
        
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = false;

                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.BearerKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            return builder;
        }
    }
}