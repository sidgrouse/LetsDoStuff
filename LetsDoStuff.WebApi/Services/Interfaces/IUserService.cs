using LetsDoStuff.WebApi.Services.DTO;

namespace LetsDoStuff.WebApi.Services.Interfaces
{
    public interface IUserService
    {
        public UserSettingsResponse GetUserSettings(string username);

        public UserResponse GetUserByUsername(string username);

        public void RegisterUser(RegisterRequest userData);
    }
}
