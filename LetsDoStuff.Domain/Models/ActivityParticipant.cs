using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoStuff.Domain.Models
{
    public class ActivityParticipant
    {
        public int Id { get; set; }

        public int ActivityId { get; set; }

        public Activity Activity { get; set; }

        public int ParticipantId { get; set; }

        public User Participant { get; set; }
    }
}
