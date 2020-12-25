using System;
using System.Collections.Generic;
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

        public List<UserSettingsResponse> GetAllUsers()
        {
            var user = _context.Users.AsNoTracking().ToList();
            var users = _context.Users.AsNoTracking()
                .Select(u => new UserSettingsResponse()
                {
                    ProfileLink = u.ProfileLink,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Bio = u.Bio ?? string.Empty,
                    DateOfBirth = u.DateOfBirth.Value.Date.ToLongDateString() ?? string.Empty,
                    DateOfRegistration = u.DateOfRegistration.Date.ToLongDateString(),
                    Role = u.Role
                }).ToList();

            return users;
        }

        public List<ActivitiesResponse> GetUserActivities(int userId)
        {
            var activities = _context.Activities.AsNoTracking().
                Where(a => a.CreatorId == userId)
                .Select(a => new ActivitiesResponse()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    DateStart = a.DateStart.ToLongDateString(),
                    Tags = a.Tags.Select(t => t.Name).ToList()
                }).ToList();

            return activities;
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
