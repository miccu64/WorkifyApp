namespace Workify.Utils.Config
{
    public record CommonConfig
    {
        public const string EnvironmentGroup = "Workify";

        public required string BearerKey { get; set; }
    }
}
