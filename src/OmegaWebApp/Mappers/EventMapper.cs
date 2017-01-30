using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmegaWebApp.Mappers
{
    public class EventMapper
    {
        //public IFormFile eventCover { get; set; }
        public string cover;
        public string location { get; set; }
        public string name { get; set; }
        public DateTime? eventStartTime { get; set; }

        public EventMapper() { }
    }
}
