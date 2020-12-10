using LetsDoStuff.WebApi.Services.DTO;

namespace LetsDoStuff.WebApi.Services.Interfaces
{
    public interface IUserService
    {
        public UserSettingsResponse GetUserSettings(string userName);

        public UserResponse GetUserByUserName(string userName);

        public void RegisterUser(RegisterRequest userData);
    }
}
