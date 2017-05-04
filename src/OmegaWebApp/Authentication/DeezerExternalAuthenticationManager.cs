using OmegaWebApp.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth;
using Omega.DAL;
using System;

namespace OmegaWebApp.Authentication
{
    public class DeezerExternalAuthenticationManager : IExternalAuthenticationManager
    {
        readonly UserService _userService;

        public DeezerExternalAuthenticationManager( UserService userService )
        {
            _userService = userService;
        }

        public async Task<string> CreateOrUpdateUser( OAuthCreatingTicketContext context )
        {
            string guid = null;
            if( context.AccessToken != null )
            {
                string deezerId = context.GetId();
                UserIndex userIndex = await _userService.FindUserIndex( "Deezer", deezerId );
                if( userIndex == null )
                {
                    guid = Guid.NewGuid().ToString();
                    await _userService.CreateUserIndex( "Deezer", deezerId, guid );

                    User user = new User();
                    user.PartitionKey = string.Empty;
                    user.RowKey = guid;
                    user.DeezerEmail = context.GetSpotifyOrDeezerEmail();
                    user.DeezerId = deezerId;
                    user.DeezerAccessToken = context.AccessToken;
                    await _userService.CreateUser( user );
                }
                else if( userIndex != null 
                    && ( await _userService.GetDeezerAccessToken( userIndex.Guid ) != context.AccessToken || await _userService.GetDeezerId( userIndex.Guid ) == null ) )
                {
                    guid = userIndex.Guid;
                    User user = new User();
                    user.PartitionKey = string.Empty;
                    user.RowKey = userIndex.Guid;
                    user.DeezerEmail = context.GetSpotifyOrDeezerEmail();
                    user.DeezerId = deezerId;
                    user.DeezerAccessToken = context.AccessToken;
                    await _userService.UpdateDeezerUser( user );
                }
                else
                    guid = userIndex.Guid;
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
