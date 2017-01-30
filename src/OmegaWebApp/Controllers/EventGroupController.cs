using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmegaWebApp.Authentication;
using Omega.DAL;
using System.Threading.Tasks;
using OmegaWebApp.Services;
using OmegaWebApp.Mappers;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.Collections.Generic;

namespace OmegaWebApp.Controllers
{
    [Route("api/[controller]")]
    [Authorize(ActiveAuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme)]
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
        public async Task CreateOmegaEvent([FromBody] EventMapper e)
        {
            string guid = User.FindFirst( "www.omega.com:guid" ).Value;
            string guidEvent = Guid.NewGuid().ToString();
            //await _eventGroupService.CreateOmegaEvent( guidEvent, guid, e.eventName, new DateTime( 2000, 1, 1 ), e.eventLocation, e.cover );
            await _eventGroupService.CreateOmegaEvent(guidEvent, guid, e.name, DateTime.Now.AddDays(3), e.location, e.cover);
        }

        [HttpPost("CreateGroup")]
        public async Task CreateOmegaGroup([FromBody] EventMapper e)
        {
            string guid = User.FindFirst("www.omega.com:guid").Value;
            string guidEvent = Guid.NewGuid().ToString();
            //await _eventGroupService.CreateOmegaEvent( guidEvent, guid, e.eventName, new DateTime( 2000, 1, 1 ), e.eventLocation, e.cover );
            string ownerPseudo = await _userService.RetrievePseudo(guid);
            await _eventGroupService.CreateOmegaGroup(guidEvent, guid, e.name, ownerPseudo);
        }

        [HttpPost( "AddMember" )]
        public async Task AddMemberToOmegaEventGroup( [FromBody]MemberAdded memberAdded )
        {
            string guid = User.FindFirst( "www.omega.com:guid" ).Value;
            User user = await _userService.FindUser( guid );
            if( !string.IsNullOrWhiteSpace( memberAdded.eventId ) && !string.IsNullOrWhiteSpace( memberAdded.pseudo ) )
            {
                EventGroup eventGroupOmega = await _eventGroupService.FindEventGroup( memberAdded.eventId, guid );
                if( eventGroupOmega.Owner )
                {
                    PseudoIndex pseudoIndex = await _userService.FindPseudoIndex( memberAdded.pseudo);
                    if( pseudoIndex != null )
                    {
                        if( eventGroupOmega.Type == "groupOmega" )
                        {
                            EventGroup groupOmega = new EventGroup( memberAdded.eventId, pseudoIndex.Guid, eventGroupOmega.Name );
                            await _eventGroupService.AddMemberToEventGroupOmega( groupOmega );
                        }
                        else if( eventGroupOmega.Type == "eventOmega" )
                        {
                            EventGroup eventOmega = new EventGroup( memberAdded.eventId, pseudoIndex.Guid, eventGroupOmega.Name, eventGroupOmega.StartTime, eventGroupOmega.Location, eventGroupOmega.Cover );
                            await _eventGroupService.AddMemberToEventGroupOmega( eventOmega );
                        }
                    }
                }
            }
        }

        public class MemberAdded
        {
            public string eventId { get; set; }
            public string pseudo { get; set; }
        }
    }
}
