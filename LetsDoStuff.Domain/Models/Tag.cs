using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LetsDoStuff.Domain.Models
{
    public class Tag
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Name of the tag.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the list of Activities having this tag.
        /// </summary>
        public List<Activity> Activities { get; set; } = new List<Activity>();
    }
}
