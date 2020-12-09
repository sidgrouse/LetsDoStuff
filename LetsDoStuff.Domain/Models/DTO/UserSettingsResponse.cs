namespace LetsDoStuff.Domain.Models.DTO
{
    public sealed class UserSettingsResponse
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Bio { get; set; }

        public string DateOfBirth { get; set; }

        public string DateOfRegistration { get; set; }

        public string Role { get; set; }
    }
}
