namespace Workify.Utils.Config
{
    public class CommonConfig
    {
        public const string EnvironmentGroup = "Workify";
        public const string JwtIssuer = "Workify";
        public const string JwtClaimUserId = "UserId";

        public required string DbConnectionString { get; set; }
        public required string BearerKey { get; set; }

        public required string RabbitMqHostname { get; set; }
        public required string RabbitMqUsername { get; set; }
        public required string RabbitMqPassword { get; set; }

        public required string SeqConnectionString { get; set; }
    }
}
