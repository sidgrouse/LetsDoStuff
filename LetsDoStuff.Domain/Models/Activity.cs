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
        /// Gets or sets the Creator.
        /// </summary>
        public User Creator { get; set; } 

        /// <summary>
        /// Gets or sets the list of ActivityTags.
        /// </summary>
        public List<ActivityTag> ActivityTags { get; set; }

        /// <summary>
        /// Gets or sets the list of Tags.
        /// </summary>
        public List<Tag> Tags { get; set; }

        public Activity()
        {
            Tags = new List<Tag>();
            ActivityTags = new List<ActivityTag>();
            Creator = new User() { Id = default, Name = "UserDefaultName", Age = default };
        }
    }
}
