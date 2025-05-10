namespace Workify.Utils.Config
{
    public class CommonConfig
    {
        public const string EnvironmentGroup = "Workify";
        public const string JwtIssuer = "Workify";
        public const string JwtClaimUserId = "UserId";

        public required string DbConnectionString { get; set; }
        public required string BearerKey { get; set; }
    }
}
