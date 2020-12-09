using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LetsDoStuff.Domain.Models
{
    [Index("Id", IsUnique = true)]
    [Index("UserName", IsUnique = true)]
    [Index("Email", IsUnique = true)]
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Bio { get; set; }

        [Required]
        public DateTime DateOfRegistration { get; set; }

        [Required]
        public string Role { get; set; }

        public User(string firstName, string lastName, string email, string password, string role)
        {
            UserName = string.Empty;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            DateOfRegistration = DateTime.Now;
            Role = role;
        }
    }
}