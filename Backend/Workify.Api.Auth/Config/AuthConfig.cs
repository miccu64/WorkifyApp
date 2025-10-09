using Workify.Utils.Config;

namespace Workify.Api.Auth.Config
{
    public class AuthConfig : CommonConfig
    {
        public required string BearerPrivateKey { get; set; }
    }
}
