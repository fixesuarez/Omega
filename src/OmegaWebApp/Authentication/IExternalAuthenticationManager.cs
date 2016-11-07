using Microsoft.AspNetCore.Authentication.OAuth;
using Omega.DAL;

namespace OmegaWebApp.Authentication
{
    public interface IExternalAuthenticationManager
    {
        void CreateOrUpdateUser( OAuthCreatingTicketContext context );

        User FindUser( OAuthCreatingTicketContext context );
    }
}
