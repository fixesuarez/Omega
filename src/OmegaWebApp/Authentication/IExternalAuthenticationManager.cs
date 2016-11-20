using Microsoft.AspNetCore.Authentication.OAuth;
using Omega.DAL;
using System.Threading.Tasks;

namespace OmegaWebApp.Authentication
{
    public interface IExternalAuthenticationManager
    {
        Task CreateOrUpdateUser( OAuthCreatingTicketContext context );

        Task<User> FindUser( OAuthCreatingTicketContext context );
    }
}
