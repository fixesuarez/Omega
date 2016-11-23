using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Omega.DAL;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OmegaWebApp.Authentication
{
    public class ExternalAuthenticationEvents
    {
        readonly IExternalAuthenticationManager _userManager;

        public ExternalAuthenticationEvents( IExternalAuthenticationManager userManager )
        {
            _userManager = userManager;
        }

        public async Task OnCreatingTicket( OAuthCreatingTicketContext context )
        {
            await _userManager.CreateOrUpdateUser( context );
            User user = await _userManager.FindUser( context );
            user.FacebookId = context.GetId();
            ClaimsPrincipal principal = CreatePrincipal( user );
            context.Ticket = new AuthenticationTicket( principal, context.Ticket.Properties, CookieAuthentication.AuthenticationScheme );
            return;
        }

        ClaimsPrincipal CreatePrincipal( User user )
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim( ClaimTypes.NameIdentifier, user.FacebookId, ClaimValueTypes.String ),
                new Claim( ClaimTypes.Email, user.Email )
            };
            ClaimsPrincipal principal = new ClaimsPrincipal( new ClaimsIdentity( claims, "Cookies", ClaimTypes.Email, string.Empty ) );
            return principal;
        }
    }
}
