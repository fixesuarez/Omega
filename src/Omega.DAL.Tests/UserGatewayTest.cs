//using Microsoft.WindowsAzure.Storage.Table;
//using NUnit.Framework;
//using System;
//using System.Threading.Tasks;

//namespace Omega.DAL.Tests
//{
//    [TestFixture]
//    public class UserGatewayTest
//    {
//        UserGateway _userGateway;

//        [SetUp]
//        public void Init()
//        {
//            _userGateway = new UserGateway( "UseDevelopmentStorage=true" );
//        }
//        [TearDown]
//        public async Task End()
//        {
//            await _userGateway.DeleteUser( "test@email.com" );
//        }

//        [Test]
//        public async Task CreateUser_and_RetrieveUser__Works_Correctly()
//        {
//            User u = new User();
//            u.PartitionKey = string.Empty;
//            u.RowKey = "test@email.com";
//            u.FacebookId = "idTest";
//            u.FacebookAccessToken = "accessTokenTest";

//            await _userGateway.CreateUser( u );

//            User retrievedUser = await _userGateway.FindUserByEmail( "test@email.com" );

//            Assert.IsNotNull( retrievedUser );
//            Assert.AreEqual( u.PartitionKey, retrievedUser.PartitionKey );
//            Assert.AreEqual( u.RowKey, retrievedUser.RowKey );
//            Assert.AreEqual( u.FacebookId, retrievedUser.FacebookId );
//            Assert.AreEqual( u.FacebookAccessToken, retrievedUser.FacebookAccessToken );
//        }
//        [Test]
//        public async Task CreateUser_With_Null_Parameter_Doesnt_Create()
//        {
//            User u = null;
//            await _userGateway.CreateUser( u );
//            Assert.ThrowsAsync<NullReferenceException>( async () => await _userGateway.FindUserByEmail( u.Email ) );
//        }
//        [Test]
//        public async Task FindIds_and_accessTokens_works_correctly()
//        {
//            User u = new User();
//            u.PartitionKey = string.Empty;
//            u.RowKey = "test@email.com";
//            u.FacebookId = "facebookId";
//            u.FacebookAccessToken = "facebookAccessToken";
//            u.DeezerId = "deezerId";
//            u.DeezerAccessToken = "deezerAccessToken";
//            u.SpotifyId = "spotifyId";
//            u.SpotifyAccessToken = "spotifyAccessToken";
//            await _userGateway.CreateUser( u );

//            User retrievedUser = await _userGateway.FindUserByEmail( u.RowKey );
//            string retriedevedFacebookId = await _userGateway.FindFacebookId( retrievedUser.Email );
//            Assert.AreEqual( u.FacebookId, retrievedUser.FacebookId );

//            string retrievedDeezerId = await _userGateway.FindDeezerId( retrievedUser.Email );
//            Assert.AreEqual( u.DeezerId, retrievedUser.DeezerId );

//            string retrievedSpotifyId = await _userGateway.FindSpotifyId( retrievedUser.Email );
//            Assert.AreEqual( u.SpotifyId, retrievedUser.SpotifyId );

//            string retrievedFacebookAccessToken = await _userGateway.FindFacebookAccessToken( retrievedUser.Email );
//            Assert.AreEqual( u.FacebookAccessToken, retrievedUser.FacebookAccessToken );

//            string retrievedDeezerAccessToken = await _userGateway.FindDeezerAccessToken( retrievedUser.Email );
//            Assert.AreEqual( u.DeezerAccessToken, retrievedUser.DeezerAccessToken );

//            string retrievedSpotifyAccessToken = await _userGateway.FindSpotifyAccessToken( retrievedUser.Email );
//            Assert.AreEqual( u.SpotifyAccessToken, retrievedUser.SpotifyAccessToken );
//        }

//        [Test]
//        public async Task UpdateUsers_works_correctly()
//        {
//            User u = new User();
//            u.PartitionKey = string.Empty;
//            u.RowKey = "test@email.com";

//            User facebookUser = new User();
//            facebookUser.PartitionKey = string.Empty;
//            facebookUser.RowKey = "test@email.com";
//            facebookUser.FacebookId = "facebookId";
//            facebookUser.FacebookAccessToken = "facebookAccessToken";
            
//            User spotifyUser = new User();
//            spotifyUser.PartitionKey = string.Empty;
//            spotifyUser.RowKey = "test@email.com";
//            spotifyUser.SpotifyId = "spotifyId";
//            spotifyUser.SpotifyAccessToken = "spotifyAccessToken";
//            spotifyUser.SpotifyRefreshToken = "spotifyRefreshToken";

//            User deezerUser = new User();
//            deezerUser.PartitionKey = string.Empty;
//            deezerUser.RowKey = "test@email.com";
//            deezerUser.DeezerId = "deezerId";
//            deezerUser.DeezerAccessToken = "deezerAccessToken";
//            deezerUser.DeezerRefreshToken = "deezerRefreshToken";

//            await _userGateway.CreateUser( u );
//            User retrievedUser = await _userGateway.FindUserByEmail( "test@email.com" );
//            Assert.IsNull( retrievedUser.FacebookId );
//            Assert.IsNull( retrievedUser.FacebookAccessToken );
//            Assert.IsNull( retrievedUser.SpotifyId );
//            Assert.IsNull( retrievedUser.SpotifyId );
//            Assert.IsNull( retrievedUser.SpotifyRefreshToken );
//            Assert.IsNull( retrievedUser.DeezerId );
//            Assert.IsNull( retrievedUser.DeezerAccessToken );
//            Assert.IsNull( retrievedUser.DeezerRefreshToken );

//            await _userGateway.UpdateFacebookUser( facebookUser );
//            retrievedUser = await _userGateway.FindUserByEmail( "test@email.com" );
//            Assert.AreEqual( facebookUser.FacebookId, retrievedUser.FacebookId );
//            Assert.AreEqual( facebookUser.FacebookAccessToken, retrievedUser.FacebookAccessToken );

//            await _userGateway.UpdateSpotifyUser( spotifyUser );
//            retrievedUser = await _userGateway.FindUserByEmail( "test@email.com" );
//            Assert.AreEqual( spotifyUser.SpotifyId, retrievedUser.SpotifyId );
//            Assert.AreEqual( spotifyUser.SpotifyAccessToken, retrievedUser.SpotifyAccessToken );
//            Assert.AreEqual( spotifyUser.SpotifyRefreshToken, retrievedUser.SpotifyRefreshToken );

//            await _userGateway.UpdateDeezerUser( deezerUser );
//            retrievedUser = await _userGateway.FindUserByEmail( "test@email.com" );
//            Assert.AreEqual( deezerUser.DeezerId, retrievedUser.DeezerId );
//            Assert.AreEqual( deezerUser.DeezerAccessToken, retrievedUser.DeezerAccessToken );
//            //Assert.AreEqual( deezerUser.DeezerRefreshToken, retrievedUser.DeezerRefreshToken );
//        }
//    }
//}
