using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LetsDoStuff.Domain.Models
{
    public class Activity
    {
        [KeyAttribute]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public int Capacity { get; set; }

        public int? Id_Creator { get; set; }
        
        public virtual User Creator { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

        public Activity()
        {
            Tags = new List<Tag>();
        }
    }
}
