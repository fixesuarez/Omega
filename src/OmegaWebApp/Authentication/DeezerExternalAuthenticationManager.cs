using OmegaWebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async void CreateOrUpdateUser( OAuthCreatingTicketContext context )
        {
            if (context.RefreshToken != null)
            {
                _userService.CreateOrUpdateDeezerUser( await _userService.FindUser( context.GetEmail() ) );
            }
        }

        public async Task<User> FindUser( OAuthCreatingTicketContext context )
        {
            return (User)await _userService.FindUser( context.GetEmail() );
        }
    }
}
