using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LetsDoStuff.Domain.Models
{
    [Index("Id", IsUnique = true)]
    [Index("ProfileLink", IsUnique = true)]
    [Index("Email", IsUnique = true)]
    public class User
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user profile link.
        /// </summary>
        [Required]
        public string ProfileLink { get; set; }

        /// <summary>
        /// Gets or sets the user first name.
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the user last name.
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the user email address.
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user password.
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the user birth date.
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the user biographic.
        /// </summary>
        public string Bio { get; set; }

        /// <summary>
        /// Gets the user registration date.
        /// </summary>
        [Required]
        public DateTime DateOfRegistration { get; }

        /// <summary>
        /// Gets or sets the user role.
        /// </summary>
        [Required]
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets activities that user want to wisit.
        /// </summary>
        public List<Activity> ParticipationActivities { get; set; } = new List<Activity>();

        /// <summary>
        /// Gets or sets activities that was created by User oneself.
        /// </summary>
        public List<Activity> OwnActivities { get; set; } = new List<Activity>();

        /// <summary>
        /// Gets or sets ParticipantsTickets that were created for checking participations.
        /// </summary>
        public List<ParticipantsTicket> ParticipantsTickets { get; set; } = new List<ParticipantsTicket>();

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="profileLink">The user profile link.</param>
        /// <param name="firstName">The user first name.</param>
        /// <param name="lastName">The user last name.</param>
        /// <param name="email">The user email address.</param>
        /// <param name="password">The user password.</param>
        /// <param name="role">The user role.</param>
        public User(string profileLink, string firstName, string lastName, string email, string password, string role)
        {
            ProfileLink = profileLink;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            DateOfRegistration = DateTime.Now;
            Role = role;
        }
    }
}