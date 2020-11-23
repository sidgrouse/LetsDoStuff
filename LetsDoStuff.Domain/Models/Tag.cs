using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LetsDoStuff.Domain.Models
{
    public class Tag
    {
        [KeyAttribute]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public virtual ICollection<Activity> Activities { get; set; }
        
        public Tag()
        {
            Activities = new List<Activity>();
        }
    }
}
