using Workify.Utils.Config;

namespace Workify.Api.Auth.Config
{
    internal record AuthConfig : CommonConfig
    {
        public required string PasswordSeed { get; set; }
    }
}
