using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LetsDoStuff.Domain.Models
{
    public class Activity
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Name of the activity.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Description of the activity.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Capacity - number of people taking part in the activity.
        /// </summary>
        [Required]
        public int Capacity { get; set; }

        /// <summary>
        /// Gets or sets the user who created the activity.
        /// </summary>
        [Required]
        public User Creator { get; set; }

        /// <summary>
        /// Gets or sets participants of the actibities.
        /// </summary>
        public List<User> Participants { get; set; } = new List<User>();

        /// <summary>
        /// Gets or sets the list of Tags.
        /// </summary>
        public List<Tag> Tags { get; set; } = new List<Tag>();

        /// <summary>
        /// Gets or sets the list of ActivityParticipant.
        /// </summary>

        /// <summary>
        /// Gets or sets the list of Users that want to take participation or attendance in the activity.
        /// </summary>
    }
}
