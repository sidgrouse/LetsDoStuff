using System;
using System.Collections.Generic;
using System.Text;
using LetsDoStuff.Domain.Models;

namespace LetsDoStuff.WebApi.SQC
{
    public sealed class GetActivitiesQueryResult
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Capacity { get; set; }

        public User Creator { get; set; }

        public List<Tag> Tags { get; set; }

        public GetActivitiesQueryResult()
        {
            Tags = new List<Tag>();
        }
    }
}
