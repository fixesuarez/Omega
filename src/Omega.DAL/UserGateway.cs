using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
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
        readonly CloudStorageAccount _storageAccount;
        readonly CloudTableClient _tableClient;
        readonly CloudTable _tableUserIndex;
        readonly CloudTable _tableUser;
        readonly CloudQueueClient _queueClient;
        readonly CloudTable tablePseudoIndex;
        readonly CloudQueue _queue;

        public UserGateway( string connectionString )
        {
            // Retrieve the storage account from the connection string.
            _storageAccount = CloudStorageAccount.Parse( connectionString );

            // Create the table client.
            _tableClient = _storageAccount.CreateCloudTableClient();

            // Create the CloudTables objects that represent the different tables.
            _tableUser = _tableClient.GetTableReference( "User" );
            // Create the table if it doesn't exist.
            _tableUser.CreateIfNotExistsAsync().Wait() ;

            _tableUserIndex = _tableClient.GetTableReference( "UserIndex" );
            _tableUserIndex.CreateIfNotExistsAsync().Wait();

            tablePseudoIndex = _tableClient.GetTableReference( "PseudoIndex" );
            tablePseudoIndex.CreateIfNotExistsAsync().Wait();
            _queueClient = _storageAccount.CreateCloudQueueClient();
            _queue = _queueClient.GetQueueReference("queue");
            _queue.CreateIfNotExistsAsync().Wait();
        }

        public async Task CreateUser( User user )
        {
            if( user != null && user.Guid != null )
            {
                User retrievedUser = await FindUser( user.Guid );
                if( retrievedUser == null )
                {
                    TableOperation insertUserOperation = TableOperation.Insert( user );
                    await _tableUser.ExecuteAsync( insertUserOperation );
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
                    await _tableUserIndex.ExecuteAsync( insertUserIndexOperation );
                }
            }
        }

        public async Task<UserIndex> FindUserIndex( string provider, string apiId )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<UserIndex>( provider, apiId );
            TableResult retrievedResult = await _tableUserIndex.ExecuteAsync( retrieveOperation );
            return (UserIndex) retrievedResult.Result;
        }
        public async Task<User> FindUser( string guid )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, guid );
            TableResult retrievedResult = await _tableUser.ExecuteAsync( retrieveOperation );
            return (User)retrievedResult.Result;
        }
        public async Task<PseudoIndex> FindPseudoIndex( string pseudo )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<PseudoIndex>( string.Empty, pseudo );
            TableResult retrievedResult = await _tableUserIndex.ExecuteAsync( retrieveOperation );
            return (PseudoIndex) retrievedResult.Result;
        }

        public async Task<string> FindFacebookId( string guid )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, guid );
            TableResult retrievedResult = await _tableUser.ExecuteAsync( retrieveOperation );
            User retrievedUser = (User) retrievedResult.Result;
            if( retrievedUser != null )
                return retrievedUser.FacebookId;
            else
                return null;
        }
        public async Task<string> FindDeezerId( string guid )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, guid );
            TableResult retrievedResult = await _tableUser.ExecuteAsync( retrieveOperation );
            User retrievedUser = (User) retrievedResult.Result;
            if( retrievedUser != null )
                return retrievedUser.DeezerId;
            else
                return null;
        }
        public async Task<string> FindSpotifyId( string email )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, email );
            TableResult retrievedResult = await _tableUser.ExecuteAsync( retrieveOperation );
            User retrievedUser = (User) retrievedResult.Result;
            if( retrievedUser != null )
                return retrievedUser.SpotifyId;
            else
                return null;
        }

        public async Task<string> FindSpotifyAccessToken( string guid )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, guid );
            TableResult retrievedResult = await _tableUser.ExecuteAsync( retrieveOperation );
            User retrievedUser = (User) retrievedResult.Result;
            if( retrievedUser != null )
                return retrievedUser.SpotifyAccessToken;
            else
                return null;
        }

        public async Task<string> FindSpotifyRefreshToken(string guid)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>(string.Empty, guid);
            TableResult retrievedResult = await _tableUser.ExecuteAsync(retrieveOperation);
            User retrievedUser = (User)retrievedResult.Result;
            if (retrievedUser != null)
                return retrievedUser.SpotifyRefreshToken;
            else
                return null;
        }
        public async Task<string> FindDeezerAccessToken( string guid )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, guid );
            TableResult retrievedResult = await _tableUser.ExecuteAsync( retrieveOperation );
            User retrievedUser = (User) retrievedResult.Result;
            if( retrievedUser != null )
                return retrievedUser.DeezerAccessToken;
            else
                return null;
        }
        public async Task<string> FindFacebookAccessToken( string guid )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, guid );
            TableResult retrievedResult = await _tableUser.ExecuteAsync( retrieveOperation );
            User retrievedUser = (User) retrievedResult.Result;
            if( retrievedUser != null )
                return retrievedUser.FacebookAccessToken;
            else
                return null;
        }
        
        public async Task DeleteUser( string guid )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, "guid" );
            TableResult retrievedResult = await _tableUser.ExecuteAsync( retrieveOperation );
            User deleteEntity = (User) retrievedResult.Result;

            if( deleteEntity != null )
            {
                TableOperation deleteOperation = TableOperation.Delete( deleteEntity );
                await _tableUser.ExecuteAsync( deleteOperation );
            }
        }

        public async Task UpdatePseudo( User u )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, u.RowKey );
            TableResult retrievedResult = await _tableUser.ExecuteAsync( retrieveOperation );
            User retrievedUser = (User) retrievedResult.Result;
            if( retrievedUser != null )
            {
                if( retrievedUser.Pseudo != null || retrievedUser.Pseudo != string.Empty )
                {
                    TableOperation retrievePseudoIndexOperation = TableOperation.Retrieve<PseudoIndex>( string.Empty, retrievedUser.Pseudo );
                    TableResult retrievedPseudoIndex = await tablePseudoIndex.ExecuteAsync( retrievePseudoIndexOperation );
                    PseudoIndex deleteEntity = (PseudoIndex) retrievedResult.Result;

                    if( deleteEntity != null )
                    {
                        TableOperation deleteOperation = TableOperation.Delete( deleteEntity );
                        await tablePseudoIndex.ExecuteAsync( deleteOperation );
                    }
                }
                retrievedUser.Pseudo = u.Pseudo;
                TableOperation updateOperation = TableOperation.Replace( retrievedUser );
                await _tableUser.ExecuteAsync( updateOperation );

                PseudoIndex pseudoIndex = new PseudoIndex( u.Pseudo, u.Guid );
                TableOperation insertOperation = TableOperation.Insert( pseudoIndex );
                await tablePseudoIndex.ExecuteAsync( insertOperation );
            }
        }

        public async Task UpdateDeezerUser( User deezerUser )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, deezerUser.RowKey );
            TableResult retrievedResult = await _tableUser.ExecuteAsync( retrieveOperation );
            User retrievedUser = (User) retrievedResult.Result;
            if( retrievedUser != null )
            {
                retrievedUser.DeezerId = deezerUser.DeezerId;
                retrievedUser.DeezerAccessToken = deezerUser.DeezerAccessToken;
                retrievedUser.DeezerEmail = deezerUser.DeezerEmail;

                TableOperation updateOperation = TableOperation.Replace( retrievedUser );
                await _tableUser.ExecuteAsync( updateOperation );
            }
            else
            {
                TableOperation insertUserOperation = TableOperation.Insert( deezerUser );
                await _tableUser.ExecuteAsync( insertUserOperation );
            }
        }
        public async Task UpdateSpotifyUser( User spotifyUser )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, spotifyUser.RowKey );
            TableResult retrievedResult = await _tableUser.ExecuteAsync( retrieveOperation );
            User retrievedUser = (User) retrievedResult.Result;
            if( retrievedUser != null )
            {
                retrievedUser.SpotifyId = spotifyUser.SpotifyId;
                retrievedUser.SpotifyEmail = spotifyUser.SpotifyEmail;
                retrievedUser.SpotifyAccessToken = spotifyUser.SpotifyAccessToken;
                retrievedUser.SpotifyRefreshToken = spotifyUser.SpotifyRefreshToken;

                TableOperation updateOperation = TableOperation.Replace( retrievedUser );
                await _tableUser.ExecuteAsync( updateOperation );
            }
            else
            {
                TableOperation insertUserOperation = TableOperation.Insert( spotifyUser );
                await _tableUser.ExecuteAsync( insertUserOperation );
            }
        }
        public async Task UpdateFacebookUser( User facebookUser)
        {
            /*
             * updating FacebookAccessToken and FacebookId If they have changed in table User
             */
            TableOperation retrieveOperation = TableOperation.Retrieve<User>(string.Empty, facebookUser.RowKey);

            // Execute the retrieve operation.
            TableResult retrievedResult = await _tableUser.ExecuteAsync(retrieveOperation);
            User retrievedUser = (User)retrievedResult.Result;

            if( retrievedUser != null )
            {
                retrievedUser.FacebookId = facebookUser.FacebookId;
                retrievedUser.FacebookEmail = facebookUser.FacebookEmail;
                retrievedUser.FacebookAccessToken = facebookUser.FacebookAccessToken;

                TableOperation updateOperation = TableOperation.Replace( retrievedUser );
                await _tableUser.ExecuteAsync( updateOperation );
            }
            else
            {
                TableOperation insertUserOperation = TableOperation.Insert( facebookUser );
                await _tableUser.ExecuteAsync( insertUserOperation );
            }
        }

        public async Task<IEnumerable<string>> GetAuthenticationProviders( string guid )
        {
            IList<string> providers = new List<string>();

            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, guid );

            // Execute the retrieve operation.
            TableResult retrievedResult = await _tableUser.ExecuteAsync( retrieveOperation );
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
            TableResult retrievedResult = await _tableUser.ExecuteAsync(retrieveOperation);
            User retrievedUser = (User)retrievedResult.Result;

            if (retrievedUser != null)
            {
                retrievedUser.GroupsId = JsonConvert.SerializeObject(groups);

                TableOperation updateOperation = TableOperation.Replace(retrievedUser);
                await _tableUser.ExecuteAsync(updateOperation);
            }
        }
        public async Task UpdateUserEvents(string guid, List<Event> events)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>(string.Empty, guid);

            // Execute the retrieve operation.
            TableResult retrievedResult = await _tableUser.ExecuteAsync(retrieveOperation);
            User retrievedUser = (User)retrievedResult.Result;

            if (retrievedUser != null)
            {
                retrievedUser.EventsId = JsonConvert.SerializeObject(events);

                TableOperation updateOperation = TableOperation.Replace(retrievedUser);
                await _tableUser.ExecuteAsync(updateOperation);
            }
        }

        public async Task<TableQuerySegment<User>> TableQueryResult()
        {
            TableContinuationToken continuationToken = null;
            TableQuery<User> tableQuery = new TableQuery<User>();
            TableQuerySegment<User> tableQueryResult;
            return tableQueryResult = await _tableUser.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);
        }

        public async Task<CloudQueueMessage> GetQueuMessage()
        {
            return await _queue.GetMessageAsync();
        }

        public async Task DeleteMessageQueue(CloudQueueMessage message)
        {
            await _queue.DeleteMessageAsync(message);
        }

        public async Task InsertQueue(string guid)
        {
            CloudQueueMessage message = new CloudQueueMessage(guid);
            await _queue.AddMessageAsync(message);
        }

        public async Task UpdateSpotifyRefreshToken(string guid, string refreshToken)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>(string.Empty, guid);
            TableResult retrievedResult = await _tableUser.ExecuteAsync(retrieveOperation);
            User retrievedUser = (User)retrievedResult.Result;

            if(retrievedUser != null)
            {
                retrievedUser.SpotifyRefreshToken = refreshToken;
                TableOperation updateOperation = TableOperation.Replace(retrievedUser);
                await _tableUser.ExecuteAsync(updateOperation);
            }
        }
    }
}
