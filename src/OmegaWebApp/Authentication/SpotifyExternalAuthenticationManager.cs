using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth;
using Omega.DAL;
using OmegaWebApp.Services;

namespace OmegaWebApp.Authentication
{
    public class SpotifyExternalAuthenticationManager : IExternalAuthenticationManager
    {
        readonly UserService _userService;

        public SpotifyExternalAuthenticationManager( UserService userService )
        {
            _userService = userService;
        }

        public async Task CreateOrUpdateUser( OAuthCreatingTicketContext context )
        {
            if( context.AccessToken != null )
            {
                User currentUser = new User( context.GetSpotifyEmail(), context.GetId(), context.AccessToken, context.RefreshToken );
                User retrievedUser = await _userService.FindUser( context.GetSpotifyEmail() );
                if( retrievedUser == null )
                    await _userService.CreateUser( currentUser );
                else if( retrievedUser.SpotifyAccessToken != currentUser.SpotifyAccessToken )
                    await _userService.UpdateSpotifyUser( currentUser );
            }
        }

        public async Task<User> FindUser( OAuthCreatingTicketContext context )
        {
            return (User)await _userService.FindUser( context.GetSpotifyEmail() );
        }
    }
}
