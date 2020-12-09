using LetsDoStuff.Domain.Models.DTO;

namespace LetsDoStuff.WebApi.Services
{
    public interface IUserService
    {
        public UserSettingsResponse GetUserSettings(string userName);

        public UserResponse GetUserByUserName(string userName);

        public void RegisterUser(RegisterRequest userData);
    }
}
