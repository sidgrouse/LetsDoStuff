using LetsDoStuff.WebApi.Services.DTO;

namespace LetsDoStuff.WebApi.Services.Interfaces
{
    public interface IUserService
    {
        public UserSettingsResponse GetUserSettings(int id);

        public UserResponse GetUserByProfileLink(string profileLink);

        public void RegisterUser(RegisterRequest userData);
    }
}
