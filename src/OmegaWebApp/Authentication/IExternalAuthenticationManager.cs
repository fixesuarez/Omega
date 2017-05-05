using Microsoft.AspNetCore.Authentication.OAuth;
using Omega.DAL;
using System.Threading.Tasks;

namespace OmegaWebApp.Authentication
{
    public interface IExternalAuthenticationManager
    {
        Task<string> CreateOrUpdateUser( OAuthCreatingTicketContext context );
        Task CreateOrUpdateUserIndex( string provider, string apiId, string guid );

        Task<User> FindUser( string guid );
    }
}