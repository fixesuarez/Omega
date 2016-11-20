using Microsoft.AspNetCore.Authentication.OAuth;
using Omega.DAL;
using OmegaWebApp.Services;
using System.Threading.Tasks;

namespace OmegaWebApp.Authentication
{
    public class FacebookExternalAuthenticationManager : IExternalAuthenticationManager
    {
        readonly UserService _userService;

        public FacebookExternalAuthenticationManager( UserService userService )
        {
            _userService = userService;
        }

        public async Task CreateOrUpdateUser( OAuthCreatingTicketContext context )
        {
            if (context.AccessToken != null)
            {
                User currentUser = new User(context.GetEmail(), string.Empty, context.AccessToken);
                User retrievedUser = await _userService.FindUser(context.GetEmail());
                if (retrievedUser == null)
                    await _userService.CreateUser(currentUser);
                else if (retrievedUser.FacebookAccessToken != currentUser.FacebookAccessToken)
                    await _userService.UpdateFacebookUser(currentUser);
            }
        }

        public async Task<User> FindUser( OAuthCreatingTicketContext context )
        {
            return await _userService.FindUser( context.GetEmail() );
        }
    }
}
