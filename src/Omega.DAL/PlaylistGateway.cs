using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
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
            _tablePlaylist = _tableClient.GetTableReference( "User" );
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
    }
}
