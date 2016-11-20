using Microsoft.AspNetCore.Authentication.OAuth;
using Omega.DAL;
using System.Threading.Tasks;

namespace OmegaWebApp.Authentication
{
    public interface IExternalAuthenticationManager
    {
        void CreateOrUpdateUser( OAuthCreatingTicketContext context );

        Task<User> FindUser( OAuthCreatingTicketContext context );
    }
}
