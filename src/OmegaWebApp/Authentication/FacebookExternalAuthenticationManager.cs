using Microsoft.AspNetCore.Authentication.OAuth;
using Omega.DAL;
using OmegaWebApp.Services;
using System;
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

        public async Task<string> CreateOrUpdateUser( OAuthCreatingTicketContext context )
        {
            string guid = null;
            if( context.AccessToken != null )
            {
                string facebookId = context.GetId();
                UserIndex userIndex = await _userService.FindUserIndex( "Facebook", facebookId );
                if( userIndex == null )
                {
                    guid = Guid.NewGuid().ToString();
                    await _userService.CreateUserIndex( "Facebook", facebookId, guid );

                    User user = new User();
                    user.PartitionKey = string.Empty;
                    user.RowKey = guid;
                    user.FacebookEmail = context.GetEmail();
                    user.FacebookId = facebookId;
                    user.FacebookAccessToken = context.AccessToken;
                    await _userService.CreateUser( user );
                }
                else if( userIndex != null 
                    && ( await _userService.GetFacebookAccessToken( userIndex.Guid ) != context.AccessToken || await _userService.GetFacebookId( userIndex.Guid ) == null ) )
                {
                    guid = userIndex.Guid;
                    User user = new User();
                    user.PartitionKey = string.Empty;
                    user.RowKey = userIndex.Guid;
                    user.FacebookEmail = context.GetEmail();
                    user.FacebookId = facebookId;
                    user.FacebookAccessToken = context.AccessToken;
                    await _userService.UpdateFacebookUser( user );
                }
                else
                    guid = userIndex.Guid;
            }
            return guid;
            //if (context.AccessToken != null)
            //{
            //    User currentUser = new User();
            //    currentUser.PartitionKey = string.Empty;
            //    currentUser.RowKey = context.GetEmail();
            //    currentUser.FacebookId = context.GetId();
            //    currentUser.FacebookAccessToken = context.AccessToken;
            //    User retrievedUser = await _userService.FindUser(context.GetEmail());
            //    if (retrievedUser == null)
            //        await _userService.CreateUser(currentUser);
            //    else if (retrievedUser.FacebookAccessToken != currentUser.FacebookAccessToken)
            //        await _userService.UpdateFacebookUser(currentUser);
            //}
        }

        public async Task CreateOrUpdateUserIndex( string provider, string apiId, string guid )
        {
            await _userService.CreateUserIndex( provider, apiId, guid );
        }

        public async Task<User> FindUser( string guid )
        {
            return await _userService.FindUser( guid );
        }
    }
}
