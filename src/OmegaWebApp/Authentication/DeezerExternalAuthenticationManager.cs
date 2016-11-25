using OmegaWebApp.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth;
using Omega.DAL;

namespace OmegaWebApp.Authentication
{
    public class DeezerExternalAuthenticationManager : IExternalAuthenticationManager
    {
        readonly UserService _userService;

        public DeezerExternalAuthenticationManager( UserService userService )
        {
            _userService = userService;
        }

        public async Task CreateOrUpdateUser( OAuthCreatingTicketContext context )
        {
            if( context.AccessToken != null )
            {
                User currentUser = new User();
                currentUser.PartitionKey = string.Empty;
                currentUser.RowKey = context.GetSpotifyOrDeezerEmail();
                currentUser.DeezerId = context.GetId();
                currentUser.DeezerAccessToken = context.AccessToken;
                User retrievedUser = await _userService.FindUser( context.GetSpotifyOrDeezerEmail() );
                if( retrievedUser == null )
                    await _userService.CreateUser( currentUser );
                else if( retrievedUser.DeezerAccessToken != currentUser.DeezerAccessToken )
                    await _userService.UpdateDeezerUser( currentUser );
            }
        }

        public async Task<User> FindUser( OAuthCreatingTicketContext context )
        {
            return (User)await _userService.FindUser( context.GetSpotifyOrDeezerEmail() );
        }
    }
}
