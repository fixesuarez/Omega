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

        public async Task CreateOmegaEvent( string guidEvent, string userGuid, string eventName, DateTime starTime, string location )
        {
            await _eventGroupGateway.CreateEventOmega( guidEvent, userGuid, eventName, starTime, location );
        }
        public async Task CreateOmegaGroup( string groupGuid, string userGuid, string groupName )
        {
            await _eventGroupGateway.CreateGroupOmega( groupGuid, userGuid, groupName );
        }
        
        public async Task<List<EventGroup>> GetAllMembersFromEventGroup(string idEventGroup )
        {
            return await _eventGroupGateway.RetrieveMembersFromGroupEvent( idEventGroup );
        }
    }
}
