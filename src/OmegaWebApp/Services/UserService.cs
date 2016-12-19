using Omega.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OmegaWebApp.Services
{
    public class UserService
    {
        readonly UserGateway _userGateway;
        readonly PasswordHasher _passwordHasher;
        readonly EventGroupGateway _eventGroupGateway;

        public UserService( UserGateway userGateway, PasswordHasher passwordHasher, EventGroupGateway eventGroupGateway )
        {
            _userGateway = userGateway;
            _passwordHasher = passwordHasher;
            _eventGroupGateway = eventGroupGateway;
        }

        public async Task<User> FindUser( string guid )
        {
            return await _userGateway.FindUser( guid );
        }
        public async Task<UserIndex> FindUserIndex(string provider, string apiId )
        {
            return await _userGateway.FindUserIndex( provider, apiId );
        }

        public async Task CreateUser( User user )
        {
            await _userGateway.CreateUser( user );
            await _eventGroupGateway.InsertPriorityQueue(user.RowKey);
        }
        public async Task CreateUserIndex( string provider, string apiId, string guid )
        {
            await _userGateway.CreateUserIndex( provider, apiId, guid );
        }
        
        public async Task UpdateSpotifyUser(User spotifyUser )
        {
            await _userGateway.UpdateSpotifyUser( spotifyUser );
        }
        public async Task UpdateDeezerUser( User deezerUser )
        {
            await _userGateway.UpdateDeezerUser( deezerUser );
        }
        public async Task UpdateFacebookUser(User facebookUser)
        {
            await _userGateway.UpdateFacebookUser(facebookUser);
            await _eventGroupGateway.InsertNormalQueue(facebookUser.RowKey);
        }
        
        public async Task<string> GetSpotifyAccessToken( string guid )
        {
            return await _userGateway.FindSpotifyAccessToken( guid );
        }
        public async Task<string> GetDeezerAccessToken( string guid )
        {
            return await _userGateway.FindDeezerAccessToken( guid );
        }
        public async Task<string> GetFacebookAccessToken( string guid )
        {
            return await _userGateway.FindFacebookAccessToken( guid );
        }

        public async Task<string> GetFacebookId( string guid )
        {
            return await _userGateway.FindFacebookId( guid );
        }
        public async Task<string> GetSpotifyId( string guid )
        {
            return await _userGateway.FindSpotifyId( guid );
        }
        public async Task<string> GetDeezerId( string guid )
        {
            return await _userGateway.FindDeezerId( guid );
        }

        public async Task<IEnumerable<string>> GetAuthenticationProviders( string guid )
        {
            return await _userGateway.GetAuthenticationProviders( guid );
        }
    }
}
