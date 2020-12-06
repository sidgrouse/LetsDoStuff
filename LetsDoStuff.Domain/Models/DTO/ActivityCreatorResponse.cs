using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoStuff.Domain.Models.DTO
{
    public sealed class ActivityCreatorResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }
    }
}
