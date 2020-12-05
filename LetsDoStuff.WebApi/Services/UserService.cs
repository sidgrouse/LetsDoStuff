using System;
using System.Linq;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
using LetsDoStuff.Domain.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace LetsDoStuff.WebApi.Services
{
    public class UserService
    {
        private readonly LdsContext _context;

        public UserService(LdsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a users by their userName.
        /// </summary>
        /// <param name="userName">User UserName.</param>
        /// <returns>Result of query.</returns>
        public GetUserByUserNameQueryResult GetUserByUserName(string userName)
        {
            var user = _context.Users.AsNoTracking()
                .Where(x => x.UserName == userName)
                .Select(x => new GetUserByUserNameQueryResult(userName)
                {
                    UserName = x.UserName,
                    ContactName = x.FirstName + " " + x.LastName,
                    Email = x.Email,
                    Bio = x.Bio,
                    DateOfBirth = x.DateOfBirth.Date.ToLongDateString(),
                    DateOfRegistration = x.DateOfRegistration.Date.ToLongDateString(),
                    Role = x.Role,
                }).FirstOrDefault();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user;
        }

        public string GetUserNameByEmail(string email)
        {
            var userName = _context.Users.AsNoTracking()
                .Where(x => x.Email == email)
                .Select(x => x.UserName).FirstOrDefault();

            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            return userName;
        }

        /// <summary>
        /// Creates a user and generate a userName with generated identity starts 1. "user1", "user2" e.g.
        /// </summary>
        /// <param name="userData">User registration data.</param>
        public void CreateUser(CreateUserCommand userData)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var newUser = new User(string.Empty, userData.FirstName, userData.LastName, userData.Email, userData.Password, "User");
                if (!string.IsNullOrWhiteSpace(userData.DateOfBirth.ToString()))
                {
                    newUser.DateOfBirth = userData.DateOfBirth;
                }

                if (!string.IsNullOrWhiteSpace(userData.Bio))
                {
                    newUser.Bio = userData.Bio;
                }

                _context.Users.Add(newUser);
                _context.SaveChanges();

                newUser.UserName = $"user{newUser.Id}";
                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
