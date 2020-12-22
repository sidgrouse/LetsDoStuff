using System.Collections.Generic;
using LetsDoStuff.WebApi.Services.DTO;

namespace LetsDoStuff.WebApi.Services.Interfaces
{
    public interface IUserService
    {
        public UserSettingsResponse GetUserSettings(int id);

        public void EditUserSettings(EditUserSettingsCommand newSettings, int id);

        public UserResponse GetUserByProfileLink(string profileLink);

        public List<UserSettingsResponse> GetAllUsers();

        public void RegisterUser(RegisterRequest userData);
    }
}
