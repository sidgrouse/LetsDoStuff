using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoStuff.Domain.Models
{
    public class ActivityAttandingUser
    {
        public int ActivityId { get; set; }

        public Activity Activity { get; set; }

        public int SubscriberId { get; set; }

        public User Subscriber { get; set; }
    }
}
