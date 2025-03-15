namespace Workify.Api.Auth.Models.Entities
{
    internal class User
    {
        public int Id { get; set; }
        public required string Login { get; set; }
        public required string HashedPassword { get; set; }
    }
}
