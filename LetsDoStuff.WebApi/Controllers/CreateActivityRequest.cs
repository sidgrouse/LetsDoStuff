using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoStuff.WebApi.Controllers
{
    public class CreateActivityRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Capacity { get; set; }

        public int IdCreator { get; set; }

        public ICollection<int> TagIds { get; set; }

        public CreateActivityRequest()
        {
            TagIds = new List<int>();
        }
    }
}
