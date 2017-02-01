using Microsoft.AspNetCore.Http;
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
        EventGroupUserGateway _eventGroupUserGateway;
        public EventGroupService(EventGroupGateway eventGroupGateway, EventGroupUserGateway eventGroupUserGateway)
        {
            _eventGroupGateway = eventGroupGateway;
            _eventGroupUserGateway = eventGroupUserGateway;
        }

        public async Task<EventGroup> FindEventGroup( string idEventGroup, string userGuid )
        {
            return await _eventGroupGateway.FindEventGroup( idEventGroup, userGuid );
        }

        public async Task CreateOmegaEvent( string guidEvent, string userGuid, string eventName, DateTime starTime, string location )
        {
            await _eventGroupGateway.CreateEventOmega( guidEvent, userGuid, eventName, starTime, location );
        }
        public async Task CreateOmegaGroup( string groupGuid, string userGuid, string groupName, string ownerPseudo )
        {
            await _eventGroupGateway.CreateGroupOmega( groupGuid, userGuid, groupName, ownerPseudo );
        }

        public async Task DeleteEventGroupOmega( string eventGroupId, string userGuid )
        {
            await _eventGroupGateway.DeleteEventGroupOmega( eventGroupId, userGuid );
        }

        public async Task DeleteOneEventGroupOmega(string eventGroupId, string userGuid)
        {
            await _eventGroupGateway.DeleteOneEventGroupOmega(eventGroupId, userGuid);
        }
        public async Task DeleteEventGroupUserOmega( string userGuid, string eventGroupGuid )
        {
            await _eventGroupUserGateway.DeleteEventGroupUserOmega( userGuid, eventGroupGuid );
        }

        public async Task DeleteOneEventGroupUserOmega(string eventGroupId, string userGuid)
        {
            await _eventGroupGateway.DeleteOneEventGroupOmega(eventGroupId, userGuid);
        }
        public async Task UploadEventGroupCover( IFormFile eventGroupCover, string eventGroupGuid, string eventGroupName )
        {
            await _eventGroupGateway.UploadEventGroupCover( eventGroupCover, eventGroupGuid, eventGroupName );
        }

        public async Task<bool> IsUserEventGroupOwner( string eventGroupId, string userGuid )
        {
            return await _eventGroupGateway.IsUserEventGroupOwner( eventGroupId, userGuid );
        }

        public async Task AddMemberToEventGroupOmega( EventGroup eventGroupOmega )
        {
            await _eventGroupGateway.AddMemberToEventGroupOmega( eventGroupOmega );
        }

        public async Task<List<EventGroup>> GetAllMembersFromEventGroup(string idEventGroup )
        {
            return await _eventGroupGateway.RetrieveMembersFromGroupEvent( idEventGroup );
        }

        public async Task<string> GetAllUserEvents(string guid, string type)
        {
            return await _eventGroupUserGateway.GetAllEventsUser(guid, type);
        }
    }
}
