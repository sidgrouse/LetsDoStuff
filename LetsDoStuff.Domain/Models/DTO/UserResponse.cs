namespace LetsDoStuff.Domain.Models.DTO
{
    public sealed class UserResponse
    {
        public string ContactName { get; set; }

        public string Email { get; set; }

        public string Bio { get; set; }

        public string DateOfBirth { get; set; }

        public string Role { get; set; }
    }
}
