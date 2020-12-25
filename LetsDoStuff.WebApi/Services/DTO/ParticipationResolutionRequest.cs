using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoStuff.WebApi.Services.DTO
{
    public class ParticipationResolutionRequest
    {
        public int ActivityId { get; set; }

        public int ParticipantId { get; set; }
    }
}
