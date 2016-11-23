using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

namespace Omega.DAL
{
    public class TrackGateway
    {
        readonly CloudStorageAccount _storageAccount;
        readonly CloudTableClient _tableClient;
        readonly CloudTable _tableTrack;
        readonly CloudQueueClient _queueClient;
        readonly CloudQueue _queue;

        public TrackGateway( string connectionString )
        {
            _storageAccount = CloudStorageAccount.Parse( connectionString );
            _tableClient = _storageAccount.CreateCloudTableClient();
            _tableTrack = _tableClient.GetTableReference( "Track" );
            _tableTrack.CreateIfNotExistsAsync();
            _queueClient = _storageAccount.CreateCloudQueueClient();
            _queue = _queueClient.GetQueueReference( "myqueue" );
            _queue.CreateIfNotExistsAsync();
        }

        public async Task InsertTrack( string userId, string source, string playlistId, string trackId, string title, string albumName, string popularity, string duration, string cover )
        {
            TableOperation retrieveTrackOperation = TableOperation.Retrieve<Track>( userId, source + ":" + playlistId + ":" + trackId );

            TableResult retrievedResult = await _tableTrack.ExecuteAsync( retrieveTrackOperation );
            Track retrievedTrack = (Track) retrievedResult.Result;
            if( retrievedTrack == null )
            {
                TableBatchOperation batchOperation = new TableBatchOperation();
                Track t = new Track( source, userId, playlistId, trackId, title, albumName, popularity, duration, cover );
                batchOperation.Insert( t );
                await _tableTrack.ExecuteBatchAsync( batchOperation );
            }
            CloudQueueMessage message = new CloudQueueMessage( source + ":" + trackId );
            await _queue.AddMessageAsync( message );
        }
    }
}
