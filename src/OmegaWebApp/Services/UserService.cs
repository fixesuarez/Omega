using Omega.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OmegaWebApp.Services
{
    public class UserService
    {
        readonly UserGateway _userGateway;
        readonly PasswordHasher _passwordHasher;

        public UserService( UserGateway userGateway, PasswordHasher passwordHasher )
        {
            _userGateway = userGateway;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> FindUser( string email )
        {
            return await _userGateway.FindUserByEmail( email );
        }

        public async Task CreateUser( User user )
        {
            await _userGateway.CreateUser( user );
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
        }
        
        public async Task<string> GetSpotifyAccessToken( string email )
        {
            return await _userGateway.FindSpotifyAccessToken( email );
        }
        public async Task<string> GetDeezerAccessToken( string email )
        {
            return await _userGateway.FindDeezerAccessToken( email );
        }
        public async Task<string> GetFacebookAccessToken( string email )
        {
            return await _userGateway.FindFacebookAccessToken( email );
        }

        public async Task<IEnumerable<string>> GetAuthenticationProviders( string userId )
        {
            return await _userGateway.GetAuthenticationProviders( userId );
        }
    }
}
