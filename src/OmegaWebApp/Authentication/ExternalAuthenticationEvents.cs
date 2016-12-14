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
            // context.Options
            string sReLoginUserId;
            if( autP.Items.TryGetValue( "reLoginUserId", out sReLoginUserId ) )
            {
                // Binding a new provider or augmenting scopes of an existing provider. 
                // I'm not sure that the "augmentScope" parameter is a good idea...
                // WE MUST be able to retrieve the scopes actually ACCEPTED by the user here!
                string augmentedScopes;
                if( autP.Items.TryGetValue( "augmentScope", out augmentedScopes ) )
                {
                    autP.Items.Remove( "augmentScope" );
                    // This where the new scopes (augmentedScopes) should be saved...
                }
            }
            // This is also where the email should be handled and account binding 
            // should occur.
            await _userManager.CreateOrUpdateUser( context );
            User user = await _userManager.FindUser( context );
            user.FacebookId = context.GetId();
            ClaimsPrincipal principal = CreatePrincipal( user );
            context.Ticket = new AuthenticationTicket( principal, autP, CookieAuthentication.AuthenticationScheme );
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
