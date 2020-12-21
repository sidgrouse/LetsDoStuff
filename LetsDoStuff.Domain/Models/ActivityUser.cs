using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LetsDoStuff.Domain.Models
{
    public class ActivityUser
    {
        public User User { get; set; }

        public int UserId { get; set; }

        public Activity Activity { get; set; }

        public int ActivityId { get; set; }

        [Required]
        public bool IsParticipante { get; set; }
    }
}
