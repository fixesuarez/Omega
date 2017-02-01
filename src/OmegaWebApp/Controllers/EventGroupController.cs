using Microsoft.AspNetCore.Mvc;
using Omega.DAL;
using System.Threading.Tasks;
using OmegaWebApp.Services;
using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace OmegaWebApp.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(ActiveAuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme)]
    public class EventGroupController : Controller
    {
        UserService _userService;
        EventGroupService _eventGroupService;
        public EventGroupController(UserService userService, EventGroupService eventGroupService )
        {
            _userService = userService;
            _eventGroupService = eventGroupService;
        }

        [HttpGet("RetrieveUserEvents")]
        public async Task<string> RetrieveUserEvents()
        {
            string guid = User.FindFirst("www.omega.com:guid").Value;
            //User user = await _userService.FindUser(guid);
            //return user.EventsId;
            return await _eventGroupService.GetAllUserEvents(guid, "event");
        }

        [HttpGet("RetrieveUserGroups")]
        public async Task<string> RetrieveUserGroups()
        {
            string guid = User.FindFirst("www.omega.com:guid").Value;
            //User user = await _userService.FindUser(guid);
            //return user.GroupsId;
            return await _eventGroupService.GetAllUserEvents(guid, "group");
        }

        [HttpPost( "CreateEvent" )]
        public async Task<JToken> CreateOmegaEvent( [FromBody] EventMapper eventCreated )
        {
            string guid = User.FindFirst( "www.omega.com:guid" ).Value;
            string guidEvent = Guid.NewGuid().ToString();
            await _eventGroupService.CreateOmegaEvent( guidEvent, guid, eventCreated.Name, eventCreated.StartTime, eventCreated.Location );
            EventGroupToSend e = new EventGroupToSend( guidEvent, eventCreated.Name );
            string eToString = JsonConvert.SerializeObject( e );
            JToken eToJson = JToken.Parse( eToString );
            return eToJson;
        }
        [HttpPost( "CreateGroup" )]
        public async Task<JToken> CreateOmegaGroup( [FromBody]string omegaGroupName )
        {
            string guid = User.FindFirst( "www.omega.com:guid" ).Value;
            string guidGroup = Guid.NewGuid().ToString();
            string ownerPseudo = await _userService.RetrievePseudo( guid );
            await _eventGroupService.CreateOmegaGroup( guidGroup, guid, omegaGroupName, ownerPseudo );
            EventGroupToSend e = new EventGroupToSend( guidGroup, omegaGroupName );
            string eToString = JsonConvert.SerializeObject( e );
            JToken eToJson = JToken.Parse( eToString );
            return eToJson;
        }

        [HttpPost( "UploadEventGroupCover/{EventGroupGuid}/{EventName}" )]
        public async Task UploadEventGroupCover( IList<IFormFile> files, string EventGroupGuid, string EventName )
        {
            await _eventGroupService.UploadEventGroupCover( files[0], EventGroupGuid, EventName );
        }

        [HttpPost( "AddMember" )]
        public async Task AddMemberToOmegaEventGroup( [FromBody]MemberAdded memberAdded )
        {
            string guid = User.FindFirst( "www.omega.com:guid" ).Value;
            User user = await _userService.FindUser( guid );
            if( !string.IsNullOrWhiteSpace( memberAdded.eventGroupId ) && !string.IsNullOrWhiteSpace( memberAdded.pseudo ) )
            {
                EventGroup eventGroupOmega = await _eventGroupService.FindEventGroup( memberAdded.eventGroupId, guid );
                if( eventGroupOmega.Owner )
                {
                    PseudoIndex pseudoIndex = await _userService.FindPseudoIndex( memberAdded.pseudo);
                    if( pseudoIndex != null )
                    {
                        if( eventGroupOmega.Type == "groupOmega" )
                        {
                            EventGroup groupOmega = new EventGroup( memberAdded.eventGroupId, pseudoIndex.Guid, eventGroupOmega.Name );
                            await _eventGroupService.AddMemberToEventGroupOmega( groupOmega );
                        }
                        else if( eventGroupOmega.Type == "eventOmega" )
                        {
                            EventGroup eventOmega = new EventGroup( memberAdded.eventGroupId, pseudoIndex.Guid, eventGroupOmega.Name, eventGroupOmega.StartTime, eventGroupOmega.Location );
                            await _eventGroupService.AddMemberToEventGroupOmega( eventOmega );
                        }
                    }
                }
            }
        }

        public class MemberAdded
        {
            public string eventGroupId { get; set; }
            public string pseudo { get; set; }
        }

        public class EventMapper
        {
            public string Location { get; set; }
            public string Name { get; set; }
            public DateTime StartTime { get; set; }
        }

        public class EventGroupToSend
        {
            public string EventGroupGuid { get; set; }
            public string EventGroupName { get; set; }
            public EventGroupToSend() { }
            public EventGroupToSend( string eventGroupGuid, string eventGroupName )
            {
                EventGroupGuid = eventGroupGuid;
                EventGroupName = eventGroupName;
            }
        }
    }
}
