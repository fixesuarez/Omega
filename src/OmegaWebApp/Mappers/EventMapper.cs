using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OmegaWebApp.Mappers
{
    public class EventMapper
    {
        public string Location { get; set; }
        public string Name { get; set; }
        public DateTime? StartTime { get; set; }

        public EventMapper() { }
    }
}
