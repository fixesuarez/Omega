using Omega.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
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

        //public bool CreateUser( string email, string password )
        //{
        //    if (_userGateway.FindUserByEmail( email ) != null) return false;
        //    _userGateway.Create( email, _passwordHasher.HashPassword( password ), string.Empty );
        //    return true;
        //}

        public async void CreateOrUpdateSpotifyUser( string email )
        {
            _userGateway.InsertOrUpdateUserBySpotify( await _userGateway.FindUserByEmail( email ) );
        }

        public void CreateOrUpdateSpotifyUser( User spotifyUser )
        {
            _userGateway.InsertOrUpdateUserBySpotify( spotifyUser );
        }

        public void CreateOrUpdateFacebookUser( User facebookUser )
        {
            _userGateway.InsertOrUpdateUserByFacebook( facebookUser );
        }

        //public async Task<User> FindUser( string email, string password )
        //{
        //    User user = await _userGateway.FindUserByEmail( email );
        //    if (user != null && _passwordHasher.VerifyHashedPassword( user.Password, password ) == PasswordVerificationResult.Success)
        //    {
        //        return await FindUser( email );
        //    }

        //    return null;
        //}

        public async Task<User> FindUser( string email )
        {
            return await _userGateway.FindUserByEmail( email );
        }

        public async Task<IEnumerable<string>> GetAuthenticationProviders( string userId )
        {
            return await _userGateway.GetAuthenticationProviders( userId );
        }
    }
}
