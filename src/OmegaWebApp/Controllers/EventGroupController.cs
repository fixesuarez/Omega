using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Omega.DAL;
using OmegaWebApp.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmegaWebApp.Controllers
{
    [Route("api/[controller]")]
    [Authorize(ActiveAuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme)]
    public class EventGroupController : Controller
    {
        UserGateway _userGateway;
        public EventGroupController(UserGateway userGateway)
        {
            _userGateway = userGateway;
        }

        [HttpGet("RetrieveUserEvents")]
        public async Task<string> RetrieveUserEvents()
        {
            string guid = User.FindFirst("www.omega.com:guid").Value;
            User user = await _userGateway.FindUser(guid);
            return user.EventsId;
        }

        [HttpGet("RetrieveUserGroups")]
        public async Task<string> RetrieveUserGroups()
        {
            string guid = User.FindFirst("www.omega.com:guid").Value;
            User user = await _userGateway.FindUser(guid);
            return user.GroupsId;
        }
    }
}
