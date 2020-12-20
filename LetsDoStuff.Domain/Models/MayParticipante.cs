using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoStuff.Domain.Models
{
    public class MayParticipante
    {
        public User CreatorActivity { get; set; }
        
        public int ParticipanteId { get; set; }

        public int ActivityId { get; set; }
    }
}
