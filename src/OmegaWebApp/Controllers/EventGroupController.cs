using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmegaWebApp.Authentication;
using Omega.DAL;
using System.Threading.Tasks;
using OmegaWebApp.Services;

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
            User user = await _userService.FindUser(guid);
            return user.EventsId;
        }

        [HttpGet("RetrieveUserGroups")]
        public async Task<string> RetrieveUserGroups()
        {
            string guid = User.FindFirst("www.omega.com:guid").Value;
            User user = await _userService.FindUser(guid);
            return user.GroupsId;
        }

        [HttpPost( "AddMember" )]
        public async Task AddMemberToOmegaEventGroup( string eventGroupId, string pseudo )
        {
            string guid = User.FindFirst( "www.omega.com:guid" ).Value;
            User user = await _userService.FindUser( guid );
            EventGroup eventGroupOmega = await _eventGroupService.FindEventGroup( eventGroupId, guid );
            if( eventGroupOmega.Owner )
            {
                PseudoIndex pseudoIndex = await _userService.FindPseudoIndex( pseudo );
                if( pseudoIndex != null )
                {
                    if( eventGroupOmega.Type == "GroupOmega" )
                    {
                        EventGroup groupOmega = new EventGroup( eventGroupId, pseudoIndex.Guid, eventGroupOmega.Name );
                        await _eventGroupService.AddMemberToEventGroupOmega( groupOmega );
                    }
                    else if( eventGroupOmega.Type == "EventOmega" )
                    {
                        EventGroup eventOmega = new EventGroup( eventGroupId, pseudoIndex.Guid, eventGroupOmega.Name, eventGroupOmega.StartTime );
                        await _eventGroupService.AddMemberToEventGroupOmega( eventOmega );
                    }
                }
            }
        }
    }
}
