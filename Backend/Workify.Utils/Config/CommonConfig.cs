namespace Workify.Utils.Config
{
    public record CommonConfig
    {
        public const string EnvironmentGroup = "Workify";

        public readonly string JwtIssuer = "Workify";
        public readonly string JwtClaimUserId = "UserId";

        public required string BearerKey { get; set; }
    }
}
