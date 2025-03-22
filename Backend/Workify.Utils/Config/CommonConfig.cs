namespace Workify.Utils.Config
{
    public class CommonConfig
    {
        public const string EnvironmentGroup = "Workify";

        public readonly string JwtIssuer = "Workify";
        public readonly string JwtClaimUserId = "UserId";

        public required string DbConnectionString { get; set; }
        public required string BearerKey { get; set; }
    }
}
