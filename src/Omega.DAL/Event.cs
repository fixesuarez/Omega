using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omega.DAL
{
    public class Event
    {
        public class Attendings
        {
            public string Email { get; set; }

            public string FacebookId { get; set; }
        }

        public string Id { get; set; }

        public List<Attendings> attendings { get; set; }

        public string Cover { get; set; }

        public string Name { get; set; }

        public DateTime StartTime { get; set; }
        public string Location { get; set; }
    }
}
