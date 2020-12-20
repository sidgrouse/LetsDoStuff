using System;
using System.Linq;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
using LetsDoStuff.WebApi.Services.DTO;
using LetsDoStuff.WebApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LetsDoStuff.WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly LdsContext _context;

        public UserService(LdsContext context)
        {
            _context = context;
        }

        public UserSettingsResponse GetUserSettings(int id)
        {
            var user = _context.Users.AsNoTracking()
                .FirstOrDefault(u => u.Id == id)
                ?? throw new ArgumentException($"User is not found");

            return new UserSettingsResponse()
            {
                ProfileLink = user.ProfileLink,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Bio = user.Bio ?? string.Empty,
                DateOfBirth = user.DateOfBirth?.Date.ToLongDateString() ?? string.Empty,
                DateOfRegistration = user.DateOfRegistration.Date.ToLongDateString(),
                Role = user.Role,
            };
        }

        public void EditUserSettings(EditUserSettingsCommand newSettings, int id)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (newSettings.Password != _context.Users.FirstOrDefault(u => u.Id == id).Password)
                {
                    throw new ArgumentException($"Incorrect password");
                }

                if (newSettings.Email != string.Empty)
                {
                    EmailValidation(newSettings.Email);
                    _context.Users.FirstOrDefault(u => u.Id == id).Email = newSettings.Email;
                }

                if (newSettings.ProfileLink != string.Empty)
                {
                    _context.Users.FirstOrDefault(u => u.Id == id).ProfileLink = newSettings.ProfileLink;
                }

                if (newSettings.FirstName != string.Empty)
                {
                    _context.Users.FirstOrDefault(u => u.Id == id).FirstName = newSettings.FirstName;
                }

                if (newSettings.LastName != string.Empty)
                {
                    _context.Users.FirstOrDefault(u => u.Id == id).LastName = newSettings.LastName;
                }

                if (newSettings.Bio != string.Empty)
                {
                    _context.Users.FirstOrDefault(u => u.Id == id).Bio = newSettings.Bio;
                }

                if (newSettings.NewPassword != string.Empty)
                {
                    _context.Users.FirstOrDefault(u => u.Id == id).Password = newSettings.NewPassword;
                }

                if (newSettings.DateOfBirth != null)
                {
                    _context.Users.FirstOrDefault(u => u.Id == id).DateOfBirth = newSettings.DateOfBirth;
                }
            }
            catch (ArgumentException ae)
            {
                transaction.Rollback();
                throw ae;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw new ArgumentException($"Settings are not changed");
            }

            _context.SaveChanges();
            transaction.Commit();
        }

        public UserResponse GetUserByProfileLink(string profileLink)
        {
            var user = _context.Users.AsNoTracking()
                .FirstOrDefault(u => u.ProfileLink == profileLink)
                ?? throw new ArgumentException($"User with this profile is not found");

            return new UserResponse()
            {
                ContactName = user.FirstName + " " + user.LastName,
                Email = user.Email,
                Bio = user.Bio ?? string.Empty,
                DateOfBirth = user.DateOfBirth?.Date.ToLongDateString() ?? string.Empty,
                Role = user.Role,
            };
        }

        public void RegisterUser(RegisterRequest userData)
        {
            UserValidation(userData);
            CreateUser(userData);
        }

        private void CreateUser(RegisterRequest userData)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                string userRoleName = "User";
                var newUser = new User(profileLink: "user", userData.FirstName, userData.LastName, userData.Email, userData.Password, userRoleName)
                {
                    DateOfBirth = userData.DateOfBirth,
                    Bio = userData.Bio
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                int generatedId = newUser.Id;
                newUser.ProfileLink = $"user{generatedId}";
                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw new ArgumentException($"User {userData.FirstName} {userData.LastName} is not created");
            }
        }

        private void UserValidation(RegisterRequest userData)
        {
            EmailValidation(userData.Email);
        }

        private void EmailValidation(string email)
        {
            if (_context.Users.AsNoTracking().Any(u => u.Email == email))
            {
                throw new ArgumentException($"User with this email is already exist");
            }
        }
    }
}
