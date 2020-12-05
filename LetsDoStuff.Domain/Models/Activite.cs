using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LetsDoStuff.Domain.Models
{
    public class Activite
    {
        [Key]
        public string Name { get; set; }

        public string Date { get; set; }

        public string Discriotion { get; set; }
    }
}
