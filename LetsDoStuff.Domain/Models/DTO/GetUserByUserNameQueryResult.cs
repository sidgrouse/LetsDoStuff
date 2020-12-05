namespace LetsDoStuff.Domain.Models.DTO
{
    public sealed class GetUserByUserNameQueryResult
    {
        public string UserName { get; set; }

        public string ContactName { get; set; }

        public string Email { get; set; }

        public string Bio { get; set; }

        public string DateOfBirth { get; set; }

        public string DateOfRegistration { get; set; }

        public string Role { get; set; }

        public GetUserByUserNameQueryResult(string userName)
        {
            UserName = userName;
        }
    }
}
