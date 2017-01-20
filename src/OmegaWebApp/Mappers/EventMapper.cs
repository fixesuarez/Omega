using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmegaWebApp.Mappers
{
    public class EventMapper
    {
        public IFormFile eventCover { get; set; }
        public string eventLocation { get; set; }
        public string eventName { get; set; }
        public DateTime? eventStartTime { get; set; }

        public EventMapper() { }
    }
}
