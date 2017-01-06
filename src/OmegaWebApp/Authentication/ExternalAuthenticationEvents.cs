using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http.Authentication;
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
            AuthenticationProperties autP = context.Ticket.Properties;
            string sReLoginUserId;
            if( autP.Items.TryGetValue( "reLoginUserId", out sReLoginUserId ) )
            {
                // Binding a new provider or augmenting scopes of an existing provider. 
                // I'm not sure that the "augmentScope" parameter is a good idea...
                // WE MUST be able to retrieve the scopes actually ACCEPTED by the user here!
                string provider = context.Options.ClaimsIssuer;
                string pGuid = autP.Items["reLoginUserId"];
                await _userManager.CreateOrUpdateUserIndex( provider, context.GetId(), pGuid );
            }
            // This is also where the email should be handled and account binding 
            // should occur.
            string guid = await _userManager.CreateOrUpdateUser( context );
            User user = await _userManager.FindUser( guid );
            ClaimsPrincipal principal = CreatePrincipal( user );
            context.Ticket = new AuthenticationTicket( principal, autP, CookieAuthentication.AuthenticationScheme );
            return;
        }

        ClaimsPrincipal CreatePrincipal( User user )
        {
            List<Claim> claims = new List<Claim>
            {
                //new Claim( ClaimTypes.NameIdentifier, user.FacebookId, ClaimValueTypes.String ),
                new Claim( "http://omega.com:guid", user.Guid )
            };
            ClaimsPrincipal principal = new ClaimsPrincipal( new ClaimsIdentity( claims, "Cookies", "http://omega.com:guid", string.Empty ) );
            return principal;
        }
    }
}
