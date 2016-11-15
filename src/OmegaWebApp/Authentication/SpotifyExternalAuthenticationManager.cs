﻿using System;
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

        public async void CreateOrUpdateUser( OAuthCreatingTicketContext context )
        {
            if (context.RefreshToken != null)
            {
                _userService.CreateOrUpdateSpotifyUser( await _userService.FindUser( context.GetEmail() ) );
            }
        }

        User IExternalAuthenticationManager.FindUser( OAuthCreatingTicketContext context )
        {
            throw new NotImplementedException();
        }

        async Task<User> FindUser( OAuthCreatingTicketContext context )
        {
            return (User)await _userService.FindUser( context.GetEmail() );
        }
    }
}