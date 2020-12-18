using System;
using System.Collections.Generic;

namespace LetsDoStuff.WebApi.Services.DTO
{
    public class CreateActivityCommand
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Capacity { get; set; }

        public List<int> TagIds { get; set; }
    }
}
