using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Workify.Utils.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder CommonApiInitialization(this WebApplicationBuilder builder)
        {
            return builder.AddJsonConfig()
                .AddJwtAuth();
        }

        private static WebApplicationBuilder AddJsonConfig(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "common-config.json"), optional: false, reloadOnChange: true);

            //builder.Configuration.AddJsonFile("common-config.json");

            return builder;
        }

        private static WebApplicationBuilder AddJwtAuth(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(x =>
                 {
                     x.RequireHttpsMetadata = false;
                     x.SaveToken = false;

                     x.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("BEARER_KEY")!)),
                         ValidateIssuer = false,
                         ValidateAudience = false,
                         ClockSkew = TimeSpan.Zero
                     };
                 });

            return builder;
        }
    }
}