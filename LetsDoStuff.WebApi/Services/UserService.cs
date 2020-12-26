using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
using LetsDoStuff.WebApi.Services.DTO;
using LetsDoStuff.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
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

        public void EditUserSettings(UpdateUserCommand updateUser)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == updateUser.IdUser);

            var newSettings = new EditUserSettingsRequest();
            updateUser.PatchDoc.ApplyTo(newSettings, updateUser.Controller.ModelState);

            if (newSettings.Password != _context.Users.FirstOrDefault(u => u.Id == updateUser.IdUser).Password)
            {
                throw new ArgumentException($"Incorrect password");
            }

            if (newSettings.Email != string.Empty && newSettings.Email != null)
            {
                EmailValidation(newSettings.Email);
                user.Email = newSettings.Email;
            }

            if (newSettings.ProfileLink != string.Empty && newSettings.ProfileLink != null)
            {
                user.ProfileLink = newSettings.ProfileLink;
            }

            if (newSettings.FirstName != string.Empty && newSettings.FirstName != null)
            {
                user.FirstName = newSettings.FirstName;
            }

            if (newSettings.LastName != string.Empty && newSettings.LastName != null)
            {
                user.LastName = newSettings.LastName;
            }

            if (newSettings.Bio != null)
            {
                user.Bio = newSettings.Bio;
            }

            if (newSettings.NewPassword != string.Empty && newSettings.NewPassword != null)
            {
                user.Password = newSettings.NewPassword;
            }

            if (newSettings.DateOfBirth != null)
            {
                user.DateOfBirth = newSettings.DateOfBirth;
            }

            _context.SaveChanges();
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
