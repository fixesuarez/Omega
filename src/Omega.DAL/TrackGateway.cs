//using Microsoft.WindowsAzure.Storage;
//using Microsoft.WindowsAzure.Storage.Queue;
//using Microsoft.WindowsAzure.Storage.Table;

//namespace Omega.DAL
//{
//    public class TrackGateway
//    {
//        readonly CloudStorageAccount _storageAccount;
//        readonly CloudTableClient _tableClient;
//        readonly CloudTable _tableTrack;
//        readonly CloudQueueClient _queueClient;
//        readonly CloudQueue _queue;

//        public TrackGateway( string connectionString )
//        {
//            // Retrieve the storage account from the connection string.
//            _storageAccount = CloudStorageAccount.Parse( connectionString );

//            // Create the table client.
//            _tableClient = _storageAccount.CreateCloudTableClient();

//            // Create the CloudTables objects that represent the different tables.
//            _tableTrack = _tableClient.GetTableReference( "Track" );

//            _queueClient = _storageAccount.CreateCloudQueueClient();
//            _queue = _queueClient.GetQueueReference( "myqueue" );
//            _queue.CreateIfNotExistsAsync();
//        }

//        public static void InsertSpotifyTrack( string userId, string playlistId, string trackId, string title, string albumName, string popularity, string duration, string cover )
//        {
//            TableOperation retrieveTrackOperation = TableOperation.Retrieve<Track>( userId, "s:" + playlistId + ":" + trackId );

//            TableResult retrievedResult = _tableTrack.ExecuteAsync( retrieveTrackOperation );
//            Track retrievedTrack = (Track)retrievedResult.Result;
//            if (retrievedTrack == null)
//            {
//                TableBatchOperation batchOperation = new TableBatchOperation();
//                Track t = new Track( "s", userId, playlistId, trackId, title, albumName, popularity, duration, cover );
//                batchOperation.Insert( t );
//                _tableTrack.ExecuteBatch( batchOperation );
//            }
//            CloudQueueMessage message = new CloudQueueMessage( "s:" + trackId );
//            _queue.AddMessageAsync( message );
//        }

//        public static void InsertDeezerTrack( string userId, string playlistId, string trackId, string title, string albumName, string popularity, string duration, string cover )
//        {
//            TableOperation retrieveTrackOperation = TableOperation.Retrieve<Track>( userId, "d:" + playlistId + ":" + trackId );
            
//            TableResult retrievedResult = _tableTrack.Execute( retrieveTrackOperation );
//            Track retrievedTrack = (Track)retrievedResult.Result;
//            if (retrievedTrack == null)
//            {
//                TableBatchOperation batchOperation = new TableBatchOperation();
//                Track t = new Track( "d", userId, playlistId, trackId, title, albumName, popularity, duration, cover );
//                batchOperation.Insert( t );
//                _tableTrack.ExecuteBatch( batchOperation );
//            }
//            CloudQueueMessage message = new CloudQueueMessage( "d:" + trackId );
//            _queue.AddMessageAsync( message );
//        }
//    }
//}
