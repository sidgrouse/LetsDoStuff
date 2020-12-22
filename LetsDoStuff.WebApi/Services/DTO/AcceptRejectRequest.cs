using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoStuff.WebApi.Services.DTO
{
    public class AcceptRejectRequest
    {
        public int ActivityId { get; set; }

        public int ParticipanteId { get; set; }
    }
}
