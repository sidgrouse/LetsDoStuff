using System.Collections.Generic;

namespace LetsDoStuff.WebApi.Services.DTO
{
    public sealed class ActivitiesResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string DateStart { get; set; }

        public List<string> Tags { get; set; }

        public ActivitiesResponse()
        {
            Tags = new List<string>();
        }
    }
}
