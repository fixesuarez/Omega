using Microsoft.AspNetCore.Authentication.OAuth;
using Omega.DAL;
using OmegaWebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async void CreateOrUpdateUser( OAuthCreatingTicketContext context )
        {
            if (context.RefreshToken != null)
            {
                _userService.CreateOrUpdateFacebookUser( await _userService.FindUser( context.GetEmail() ) );
            }
        }

        public async Task<User> FindUser( OAuthCreatingTicketContext context )
        {
            return (User) await _userService.FindUser( context.GetEmail() );
        }

        User IExternalAuthenticationManager.FindUser( OAuthCreatingTicketContext context )
        {
            throw new NotImplementedException();
        }
    }
}
