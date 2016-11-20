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

        //public async void CreateOrUpdateUser( OAuthCreatingTicketContext context )
        //{
        //    if (context.AccessToken != null)
        //    {
        //        _userService.CreateOrUpdateFacebookUser( await _userService.FindUser( context.GetEmail() ) );
        //    }
        //}

        public async void CreateOrUpdateUser( OAuthCreatingTicketContext context )
        {
            //User currentUser = new User(context.GetEmail(), 

            //if (context.AccessToken != null)
            //{
            //    User currentUser = await _userService.FindUser( context.GetEmail() );
            //    if( currentUser == null )
            //        //_userService.CreateFacebookUser();
            //    _userService.CreateOrUpdateFacebookUser( await _userService.FindUser( context.GetEmail() ) );
            //}
        }

        public async Task<User> FindUser( OAuthCreatingTicketContext context )
        {
            return await _userService.FindUser( context.GetEmail() );
        }
    }
}
