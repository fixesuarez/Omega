using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Omega.DAL
{
    public class UserGateway
    {
        readonly CloudStorageAccount storageAccount;
        readonly CloudTableClient tableClient;
        readonly CloudTable tableUserIndex;
        readonly CloudTable tableUser;

        public UserGateway( string connectionString )
        {
            // Retrieve the storage account from the connection string.
            storageAccount = CloudStorageAccount.Parse( connectionString );

            // Create the table client.
            tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTables objects that represent the different tables.
            tableUser = tableClient.GetTableReference( "User" );
            // Create the table if it doesn't exist.
            tableUser.CreateIfNotExistsAsync().Wait() ;

            tableUserIndex = tableClient.GetTableReference( "UserIndex" );
            tableUserIndex.CreateIfNotExistsAsync().Wait();
        }
        
        public async Task CreateUser( User user )
        {
            if( user != null && user.Guid != null )
            {
                User retrievedUser = await FindUser( user.Guid );
                if( retrievedUser == null )
                {
                    TableOperation insertUserOperation = TableOperation.Insert( user );
                    await tableUser.ExecuteAsync( insertUserOperation );
                }
            }
        }
        public async Task CreateUserIndex( string provider, string apiId, string guid )
        {
            if( provider != null && apiId != null )
            {
                if( await FindUserIndex( provider, apiId ) == null )
                {
                    UserIndex userIndex = new UserIndex();
                    userIndex.PartitionKey = provider;
                    userIndex.RowKey = apiId;
                    userIndex.Guid = guid;
                    TableOperation insertUserIndexOperation = TableOperation.Insert( userIndex );
                    await tableUserIndex.ExecuteAsync( insertUserIndexOperation );
                }
            }
        }

        public async Task<UserIndex> FindUserIndex( string provider, string apiId )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<UserIndex>( provider, apiId );
            TableResult retrievedResult = await tableUserIndex.ExecuteAsync( retrieveOperation );
            return (UserIndex) retrievedResult.Result;
        }
        public async Task<User> FindUser( string guid )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, guid );
            TableResult retrievedResult = await tableUser.ExecuteAsync( retrieveOperation );
            return (User)retrievedResult.Result;
        }

        public async Task<string> FindFacebookId( string guid )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, guid );
            TableResult retrievedResult = await tableUser.ExecuteAsync( retrieveOperation );
            User retrievedUser = (User) retrievedResult.Result;
            if( retrievedUser != null )
                return retrievedUser.FacebookId;
            else
                return null;
        }
        public async Task<string> FindDeezerId( string guid )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, guid );
            TableResult retrievedResult = await tableUser.ExecuteAsync( retrieveOperation );
            User retrievedUser = (User) retrievedResult.Result;
            if( retrievedUser != null )
                return retrievedUser.DeezerId;
            else
                return null;
        }
        public async Task<string> FindSpotifyId( string email )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, email );
            TableResult retrievedResult = await tableUser.ExecuteAsync( retrieveOperation );
            User retrievedUser = (User) retrievedResult.Result;
            if( retrievedUser != null )
                return retrievedUser.SpotifyId;
            else
                return null;
        }

        public async Task<string> FindSpotifyAccessToken( string guid )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, guid );
            TableResult retrievedResult = await tableUser.ExecuteAsync( retrieveOperation );
            User retrievedUser = (User) retrievedResult.Result;
            if( retrievedUser != null )
                return retrievedUser.SpotifyAccessToken;
            else
                return null;
        }

        public async Task<string> FindSpotifyRefreshToken(string guid)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>(string.Empty, guid);
            TableResult retrievedResult = await tableUser.ExecuteAsync(retrieveOperation);
            User retrievedUser = (User)retrievedResult.Result;
            if (retrievedUser != null)
                return retrievedUser.SpotifyRefreshToken;
            else
                return null;
        }
        public async Task<string> FindDeezerAccessToken( string guid )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, guid );
            TableResult retrievedResult = await tableUser.ExecuteAsync( retrieveOperation );
            User retrievedUser = (User) retrievedResult.Result;
            if( retrievedUser != null )
                return retrievedUser.DeezerAccessToken;
            else
                return null;
        }
        public async Task<string> FindFacebookAccessToken( string guid )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, guid );
            TableResult retrievedResult = await tableUser.ExecuteAsync( retrieveOperation );
            User retrievedUser = (User) retrievedResult.Result;
            if( retrievedUser != null )
                return retrievedUser.FacebookAccessToken;
            else
                return null;
        }
        
        public async Task DeleteUser( string guid )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, "guid" );
            TableResult retrievedResult = await tableUser.ExecuteAsync( retrieveOperation );
            User deleteEntity = (User) retrievedResult.Result;

            if( deleteEntity != null )
            {
                TableOperation deleteOperation = TableOperation.Delete( deleteEntity );
                await tableUser.ExecuteAsync( deleteOperation );
            }
        }

        public async Task UpdateDeezerUser( User deezerUser )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, deezerUser.RowKey );
            TableResult retrievedResult = await tableUser.ExecuteAsync( retrieveOperation );
            User retrievedUser = (User) retrievedResult.Result;
            if( retrievedUser != null )
            {
                retrievedUser.DeezerId = deezerUser.DeezerId;
                retrievedUser.DeezerAccessToken = deezerUser.DeezerAccessToken;
                retrievedUser.DeezerEmail = deezerUser.DeezerEmail;

                TableOperation updateOperation = TableOperation.Replace( retrievedUser );
                await tableUser.ExecuteAsync( updateOperation );
            }
            else
            {
                TableOperation insertUserOperation = TableOperation.Insert( deezerUser );
                await tableUser.ExecuteAsync( insertUserOperation );
            }
        }
        public async Task UpdateSpotifyUser( User spotifyUser )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, spotifyUser.RowKey );
            TableResult retrievedResult = await tableUser.ExecuteAsync( retrieveOperation );
            User retrievedUser = (User) retrievedResult.Result;
            if( retrievedUser != null )
            {
                retrievedUser.SpotifyId = spotifyUser.SpotifyId;
                retrievedUser.SpotifyEmail = spotifyUser.SpotifyEmail;
                retrievedUser.SpotifyAccessToken = spotifyUser.SpotifyAccessToken;
                retrievedUser.SpotifyRefreshToken = spotifyUser.SpotifyRefreshToken;

                TableOperation updateOperation = TableOperation.Replace( retrievedUser );
                await tableUser.ExecuteAsync( updateOperation );
            }
            else
            {
                TableOperation insertUserOperation = TableOperation.Insert( spotifyUser );
                await tableUser.ExecuteAsync( insertUserOperation );
            }
        }
        public async Task UpdateFacebookUser( User facebookUser)
        {
            /*
             * updating FacebookAccessToken and FacebookId If they have changed in table User
             */
            TableOperation retrieveOperation = TableOperation.Retrieve<User>(string.Empty, facebookUser.RowKey);

            // Execute the retrieve operation.
            TableResult retrievedResult = await tableUser.ExecuteAsync(retrieveOperation);
            User retrievedUser = (User)retrievedResult.Result;

            if( retrievedUser != null )
            {
                retrievedUser.FacebookId = facebookUser.FacebookId;
                retrievedUser.FacebookEmail = facebookUser.FacebookEmail;
                retrievedUser.FacebookAccessToken = facebookUser.FacebookAccessToken;

                TableOperation updateOperation = TableOperation.Replace( retrievedUser );
                await tableUser.ExecuteAsync( updateOperation );
            }
            else
            {
                TableOperation insertUserOperation = TableOperation.Insert( facebookUser );
                await tableUser.ExecuteAsync( insertUserOperation );
            }
        }

        public async Task<IEnumerable<string>> GetAuthenticationProviders( string guid )
        {
            IList<string> providers = new List<string>();

            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, guid );

            // Execute the retrieve operation.
            TableResult retrievedResult = await tableUser.ExecuteAsync( retrieveOperation );
            User user = (User) retrievedResult.Result;
            if( user == null )
                return providers;
            if( user.DeezerId != null )
                providers.Add( "Deezer" );
            if( user.SpotifyId != null )
                providers.Add( "Spotify" );
            if( user.FacebookId != null )
                providers.Add( "Facebook" );

            return providers;
        }

        public async Task UpdateUserGroups(string guid, JArray groups)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>(string.Empty, guid);

            // Execute the retrieve operation.
            TableResult retrievedResult = await tableUser.ExecuteAsync(retrieveOperation);
            User retrievedUser = (User)retrievedResult.Result;

            if (retrievedUser != null)
            {
                retrievedUser.GroupsId = JsonConvert.SerializeObject(groups);

                TableOperation updateOperation = TableOperation.Replace(retrievedUser);
                await tableUser.ExecuteAsync(updateOperation);
            }
        }

        public async Task UpdateUserEvents(string guid, List<Event> events)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>(string.Empty, guid);

            // Execute the retrieve operation.
            TableResult retrievedResult = await tableUser.ExecuteAsync(retrieveOperation);
            User retrievedUser = (User)retrievedResult.Result;

            if (retrievedUser != null)
            {
                retrievedUser.EventsId = JsonConvert.SerializeObject(events);

                TableOperation updateOperation = TableOperation.Replace(retrievedUser);
                await tableUser.ExecuteAsync(updateOperation);
            }
        }
    }
}
