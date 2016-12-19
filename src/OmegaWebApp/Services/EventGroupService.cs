using Omega.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmegaWebApp.Services
{
    public class EventGroupService
    {
        EventGroupGateway _eventGroupGateway;
        public EventGroupService(EventGroupGateway eventGroupGateway)
        {
            _eventGroupGateway = eventGroupGateway;
        }
        
    }
}
