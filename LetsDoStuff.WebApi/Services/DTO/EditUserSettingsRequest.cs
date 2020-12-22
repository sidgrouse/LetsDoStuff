using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LetsDoStuff.WebApi.Services.DTO
{
    public class EditUserSettingsRequest
    {
        public string ProfileLink { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Bio { get; set; }

        public string NewPassword { get; set; }

        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
