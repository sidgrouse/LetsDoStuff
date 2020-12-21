using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoStuff.WebApi.Services.DTO
{
    public class ParticipationResponse
    {
        public string ContactName { get; set; }

        public string Email { get; set; }

        public string ProfileLink { get; set; }

        public string ActivityName { get; set; }

        public int ActicityId { get; set; }
    }
}
