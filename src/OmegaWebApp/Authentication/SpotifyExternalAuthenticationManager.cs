using System;
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

        public async Task<string> CreateOrUpdateUser( OAuthCreatingTicketContext context )
        {
            string guid = null;
            if( context.AccessToken != null )
            {
                string spotifyId = context.GetId();
                UserIndex userIndex = await _userService.FindUserIndex( "Spotify", spotifyId );
                if( userIndex == null )
                {
                    guid = Guid.NewGuid().ToString();
                    await _userService.CreateUserIndex( "Spotify", spotifyId, guid );

                    User user = new User();
                    user.PartitionKey = string.Empty;
                    user.RowKey = guid;
                    user.SpotifyEmail = context.GetSpotifyOrDeezerEmail();
                    user.SpotifyId = spotifyId;
                    user.SpotifyAccessToken = context.AccessToken;
                    user.SpotifyRefreshToken = context.RefreshToken;
                    await _userService.CreateUser( user );
                }
                else if( userIndex != null 
                    && ( await _userService.GetSpotifyAccessToken( userIndex.Guid ) != context.AccessToken || _userService.GetSpotifyId(userIndex.Guid) == null ) )
                {
                    User user = await _userService.FindUser( userIndex.Guid );
                    guid = userIndex.Guid;
                    user.PartitionKey = string.Empty;
                    user.RowKey = userIndex.Guid;
                    user.SpotifyEmail = context.GetSpotifyOrDeezerEmail();
                    user.SpotifyId = spotifyId;
                    user.SpotifyAccessToken = context.AccessToken;
                    user.SpotifyRefreshToken = context.RefreshToken;
                    await _userService.UpdateSpotifyUser( user );
                }
                else
                    guid = userIndex.Guid;
                //if( context.AccessToken != null )
                //{
                //    User currentUser = new User();
                //    currentUser.PartitionKey = string.Empty;
                //    currentUser.RowKey = context.GetSpotifyOrDeezerEmail();
                //    currentUser.SpotifyId = context.GetId();
                //    currentUser.SpotifyAccessToken = context.AccessToken;
                //    currentUser.SpotifyRefreshToken = context.RefreshToken;
                //    User retrievedUser = await _userService.FindUser( context.GetSpotifyOrDeezerEmail() );
                //    if( retrievedUser == null )
                //        await _userService.CreateUser( currentUser );
                //    else if( retrievedUser.SpotifyAccessToken != currentUser.SpotifyAccessToken )
                //        await _userService.UpdateSpotifyUser( currentUser );
                //}
            }
            return guid;
        }

        public async Task CreateOrUpdateUserIndex( string provider, string apiId, string guid )
        {
            await _userService.CreateUserIndex( provider, apiId, guid );
        }

        public async Task<User> FindUser( string guid )
        {
            return (User)await _userService.FindUser( guid );
        }
    }
}
