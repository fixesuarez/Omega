using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Omega.DAL
{
    public class PlaylistGateway
    {
        readonly CloudStorageAccount _storageAccount;
        readonly CloudTableClient _tableClient;
        readonly CloudTable _tablePlaylist;

        public PlaylistGateway( string connectionString )
        {
            _storageAccount = CloudStorageAccount.Parse( connectionString );
            _tableClient = _storageAccount.CreateCloudTableClient();
            _tablePlaylist = _tableClient.GetTableReference( "Playlist" );
            _tablePlaylist.CreateIfNotExistsAsync();
        }

        public async Task InsertPlaylist( Playlist p )
        {
            TableOperation retrievePlaylistOperation = TableOperation.Retrieve<Playlist>( p.PartitionKey, p.RowKey );

            TableResult retrievedResult = await _tablePlaylist.ExecuteAsync( retrievePlaylistOperation );
            if( retrievedResult.Result == null )
            {
                TableBatchOperation batchOperation = new TableBatchOperation();
                batchOperation.Insert( p );
                await _tablePlaylist.ExecuteBatchAsync( batchOperation );
            }
        }
        
        public async Task<List<Playlist>> RetrieveAllPlaylistsFromUser( string spotifyId, string deezerId )
        {
            List<Playlist> playlists = new List<Playlist>();
            if( spotifyId != null )
            {
                TableQuery<Playlist> query = new TableQuery<Playlist>()
                    .Where( TableQuery.GenerateFilterCondition( "PartitionKey", QueryComparisons.Equal, spotifyId ) );

                query.TakeCount = 1000;
                TableContinuationToken tableContinuationToken = null;
                do
                {
                    var queryResponse = await _tablePlaylist.ExecuteQuerySegmentedAsync( query, tableContinuationToken );
                    tableContinuationToken = queryResponse.ContinuationToken;
                    playlists.AddRange( queryResponse.Results );
                } while( tableContinuationToken != null );
            }
            else if( deezerId != null )
            {
                TableQuery<Playlist> query = new TableQuery<Playlist>()
                    .Where( TableQuery.GenerateFilterCondition( "PartitionKey", QueryComparisons.Equal, spotifyId ) );

                query.TakeCount = 1000;
                TableContinuationToken tableContinuationToken = null;
                do
                {
                    var queryResponse = await _tablePlaylist.ExecuteQuerySegmentedAsync( query, tableContinuationToken );
                    tableContinuationToken = queryResponse.ContinuationToken;
                    playlists.AddRange( queryResponse.Results );
                } while( tableContinuationToken != null );
            }
            return playlists;
        }
    }
}
