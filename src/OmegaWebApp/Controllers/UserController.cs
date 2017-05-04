using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OmegaWebApp.Authentication;
using OmegaWebApp.Services;
using Omega.DAL;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

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
            string pseudo =  await _userService.FindUserPseudo( guid );
            PseudoSender pseudoSender = new PseudoSender( pseudo );
            string pseudoToSend = JsonConvert.SerializeObject( pseudoSender );
            JToken pseudoJToken = JToken.Parse( pseudoToSend );
            return pseudoJToken;
        }
        [HttpGet( "RetrieveAllPseudos" )]
        public async Task<JToken> RetrieveAllPseudo()
        {
            List<PseudoIndex> allPseudos = await _userService.FindAllPseudos();
            string allPseudosStringified = JsonConvert.SerializeObject( allPseudos );
            JToken allPseudoJToken = JToken.Parse( allPseudosStringified );
            return allPseudoJToken;
        }

        class PseudoSender
        {
            public string Pseudo { get; set; }
            internal PseudoSender( string pseudo )
            {
                Pseudo = pseudo;
            }
        }
    }
}
