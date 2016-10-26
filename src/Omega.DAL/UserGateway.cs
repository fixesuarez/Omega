using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omega.DAL
{
    public class UserGateway
    {
        static readonly CloudStorageAccount storageAccount;
        static readonly CloudTableClient tableClient;
        static readonly CloudTable tableUser;
        static readonly CloudTable tableFacebookUser;

        public UserGateway() { }
        static UserGateway()
        {
            // Retrieve the storage account from the connection string.
            storageAccount = CloudStorageAccount.Parse( "StorageConnectionString" );

            // Create the table client.
            tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTables objects that represent the different tables.
            tableUser = tableClient.GetTableReference( "User" );
            tableFacebookUser = tableClient.GetTableReference( "FacebookUser" );
        }

        public async void InsertOrUpdateUserBySpotify( User spotifyUser )
        {
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, spotifyUser.RowKey );

            // Execute the retrieve operation.
            TableResult retrievedResult = await tableUser.ExecuteAsync( retrieveOperation );
            User retrievedUser = (User)retrievedResult.Result;

            if (retrievedResult.Result != null && (retrievedUser.SpotifyId != spotifyUser.SpotifyId || retrievedUser.SpotifyAccessToken != spotifyUser.SpotifyAccessToken))
            {
                retrievedUser.SpotifyRefreshToken = spotifyUser.SpotifyRefreshToken;
                retrievedUser.SpotifyAccessToken = spotifyUser.SpotifyAccessToken;
                retrievedUser.SpotifyId = spotifyUser.SpotifyId;

                TableOperation updateOperation = TableOperation.Replace( retrievedUser );
                await tableUser.ExecuteAsync( updateOperation );
            }
            else if (retrievedUser == null)
            {
                TableOperation insertOperation = TableOperation.Insert( spotifyUser );
                await tableUser.ExecuteAsync( insertOperation );
            }
        }

        public async void InsertOrUpdateUserByDeezer( User deezerUser )
        {
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, deezerUser.RowKey );

            // Execute the retrieve operation.
            TableResult retrievedResult = await tableUser.ExecuteAsync( retrieveOperation );
            User retrievedUser = (User)retrievedResult.Result;

            if (retrievedResult.Result != null && (retrievedUser.DeezerId != deezerUser.DeezerId || retrievedUser.DeezerAccessToken != deezerUser.DeezerAccessToken))
            {
                retrievedUser.DeezerAccessToken = deezerUser.DeezerAccessToken;
                retrievedUser.DeezerId = deezerUser.DeezerId;

                TableOperation updateOperation = TableOperation.Replace( retrievedUser );
                await tableUser.ExecuteAsync( updateOperation );
            }
            else if (retrievedUser == null)
            {
                TableOperation insertOperation = TableOperation.Insert( deezerUser );
                await tableUser.ExecuteAsync( insertOperation );
            }
        }

        public async void InsertOrUpdateUserByFacebook( User facebookUser )
        {
            // Create a retrieve operation that takes a User entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, facebookUser.RowKey );

            // Execute the retrieve operation.
            TableResult retrievedResult = await tableUser.ExecuteAsync( retrieveOperation );
            User retrievedUser = (User)retrievedResult.Result;

            if (retrievedResult.Result != null && (retrievedUser.FacebookId != facebookUser.FacebookId || retrievedUser.FacebookAccessToken != facebookUser.FacebookAccessToken))
            {
                retrievedUser.FacebookId = facebookUser.FacebookId;
                retrievedUser.FacebookAccessToken = facebookUser.FacebookAccessToken;

                TableOperation updateOperation = TableOperation.Replace( retrievedUser );
                await tableUser.ExecuteAsync( updateOperation );
            }
            else if (retrievedUser == null)
            {
                TableOperation insertOperation = TableOperation.Insert( facebookUser );
                await tableUser.ExecuteAsync( insertOperation );
            }

            TableOperation retrieveFacebookUserOperation = TableOperation.Retrieve<FacebookUser>( string.Empty, facebookUser.FacebookId );
            TableResult retrievedFacebookUserResult = await tableFacebookUser.ExecuteAsync( retrieveFacebookUserOperation );
            FacebookUser retrievedFacebookUser = (FacebookUser)retrievedFacebookUserResult.Result;
            if (retrievedFacebookUser == null)
            {
                FacebookUser fUser = new FacebookUser( facebookUser.FacebookId, facebookUser.Email );
                TableOperation insertFacebookUser = TableOperation.Insert( fUser );
                await tableFacebookUser.ExecuteAsync( insertFacebookUser );
            }
        }
    }
}
