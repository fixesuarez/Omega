using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OmegaWebApp.Authentication;
using OmegaWebApp.Services;
using Omega.DAL;
using Newtonsoft.Json.Linq;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OmegaWebApp.Controllers
{
    [Route( "api/[controller]" )]
    [Authorize( ActiveAuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme )]
    public class UserController : Controller
    {
        readonly UserService _userService;

        public UserController( UserService userService )
        {
            _userService = userService;
        }

        [HttpPost( "UpdatePseudo" )]
        public async Task UpdatePseudo( [FromBody]string pseudo )
        {
            if( !string.IsNullOrWhiteSpace(pseudo ) )
            {
                string guid = User.FindFirst( "www.omega.com:guid" ).Value;
                User user = await _userService.FindUser( guid );
                user.Pseudo = pseudo;
                await _userService.UpdatePseudo( user );
            }
        }

        [HttpGet( "RetrievePseudo")]
        public async Task<JToken> RetrievePseudo()
        {
            string guid = User.FindFirst( "www.omega.com:guid" ).Value;
            string pseudo = await _userService.FindUserPseudo( guid );
            JToken pseudoJToken = JToken.Parse( pseudo );
            return pseudoJToken;
        }
    }
}
