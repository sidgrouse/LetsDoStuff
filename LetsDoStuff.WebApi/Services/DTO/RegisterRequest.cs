using System;
using System.ComponentModel.DataAnnotations;

namespace LetsDoStuff.WebApi.Services.DTO
{
    public sealed class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Bio { get; set; }
    }
}
