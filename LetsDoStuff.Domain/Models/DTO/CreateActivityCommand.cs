using System;
using System.Collections.Generic;

namespace LetsDoStuff.Domain.Models.DTO
{
    public class CreateActivityCommand 
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Capacity { get; set; }

        public int CreatorId { get; set; }

        public List<int> TagIds { get; set; }
    }
}
