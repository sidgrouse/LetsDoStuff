using System;
using System.Linq;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
using LetsDoStuff.Domain.Models.DTO;
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

        public UserSettingsResponse GetUserSettings(string userName)
        {
            var user = _context.Users.AsNoTracking()
                .FirstOrDefault(u => u.UserName == userName)
                ?? throw new ArgumentException($"User is not found");

            return new UserSettingsResponse()
            {
                UserName = userName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Bio = user.Bio ?? string.Empty,
                DateOfBirth = user.DateOfBirth.Date.ToLongDateString(),
                DateOfRegistration = user.DateOfRegistration.Date.ToLongDateString(),
                Role = user.Role,
            };
        }

        public UserResponse GetUserByUserName(string userName)
        {
            var user = _context.Users.AsNoTracking()
                .FirstOrDefault(u => u.UserName == userName)
                ?? throw new ArgumentException($"User with userName: {userName} is not found");

            return new UserResponse()
            {
                ContactName = user.FirstName + " " + user.LastName,
                Email = user.Email,
                Bio = user.Bio ?? string.Empty,
                DateOfBirth = user.DateOfBirth.Date.ToLongDateString(),
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
                var newUser = new User(userData.FirstName, userData.LastName, userData.Email, userData.Password, "User")
                {
                    DateOfBirth = userData.DateOfBirth,
                    Bio = userData.Bio
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                newUser.UserName = $"user{newUser.Id}";
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
