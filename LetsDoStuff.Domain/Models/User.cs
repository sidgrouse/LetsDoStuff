using System.Collections.Generic;

namespace LetsDoStuff.Domain.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public int Age { get; set; }

        public string Role { get; set; }

        public List<Activity> CreatedActivities { get; set; } = new List<Activity>();

        /// <summary>
        /// Gets or sets the list of ActivityParticipant.
        /// </summary>
        public List<ActivityParticipant> ActivityParticipants { get; set; } = new List<ActivityParticipant>();

        /// <summary>
        /// Gets or sets the list of Activities for participation or attendance.
        /// </summary>
        public List<Activity> ActivitiesForParticipation { get; set; } = new List<Activity>();
    }
}
