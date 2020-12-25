using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoStuff.WebApi.Services.DTO
{
    public class ParticipantResponse
    {
        public string ContactName { get; set; }

        public string Email { get; set; }

        public string ProfileLink { get; set; }

        public int ContactId { get; set; }

        public string ActivityName { get; set; }

        public int ActicityId { get; set; }

        public bool AcceptAsParticipant { get; set; }
    }
}
