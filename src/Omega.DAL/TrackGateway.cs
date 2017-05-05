using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
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
            _tableTrack.CreateIfNotExistsAsync().Wait();
            _queueClient = _storageAccount.CreateCloudQueueClient();
            _queue = _queueClient.GetQueueReference( "myqueue" );
            _queue.CreateIfNotExistsAsync().Wait();
        }

        public async Task InsertTrack( string source, string playlistId, string trackId, string title, string albumName, string popularity, string duration, string cover )
        {
            try
            {
                TableOperation retrieveTrackOperation = TableOperation.Retrieve<Track>(playlistId, source + ":" + playlistId + ":" + trackId);

                TableResult retrievedResult = await _tableTrack.ExecuteAsync(retrieveTrackOperation);
                Track retrievedTrack = (Track)retrievedResult.Result;
                if (retrievedTrack == null)
                {
                    TableBatchOperation batchOperation = new TableBatchOperation();
                    Track t = new Track(source, playlistId, trackId, title, albumName, popularity, duration, cover);
                    batchOperation.Insert(t);
                    await _tableTrack.ExecuteBatchAsync(batchOperation);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public async Task DeleteTrack( string playlistId, string source, string trackId )
        {
            try
            {
                TableOperation retrieveOperation = TableOperation.Retrieve<Track>(playlistId, string.Format("{0}:{1}:{2}", source, playlistId, trackId));
                TableResult retrievedResult = await _tableTrack.ExecuteAsync(retrieveOperation);
                Track deleteEntity = (Track)retrievedResult.Result;

                if (deleteEntity != null)
                {
                    TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                    await _tableTrack.ExecuteAsync(deleteOperation);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task<Track> RetrieveTrack( string source, string idPlaylist, string idTrack )
        {
            try
            {
                string rowKey = source + ":" + idPlaylist + ":" + idTrack;
                TableOperation retrieveOperation = TableOperation.Retrieve<Track>(idPlaylist, rowKey);
                TableResult retrievedResult = await _tableTrack.ExecuteAsync(retrieveOperation);
                Track retrievedUser = (Track)retrievedResult.Result;
                return retrievedUser;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            
        }

        public async Task DeleteAllTrackPlaylist(string playlistId)
        {
            try
            {
                TableBatchOperation batchOperation = new TableBatchOperation();
                List<Track> tracks = new List<Track>();
                TableQuery<Track> query = new TableQuery<Track>()
                        .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, playlistId));

                query.TakeCount = 1000;
                TableContinuationToken tableContinuationToken = null;
                do
                {
                    var queryResponse = await _tableTrack.ExecuteQuerySegmentedAsync(query, tableContinuationToken);
                    tableContinuationToken = queryResponse.ContinuationToken;
                    tracks.AddRange(queryResponse.Results);
                } while (tableContinuationToken != null);

                foreach (Track track in tracks)
                {
                    batchOperation.Delete(track);
                    if(batchOperation.Count >= 50)
                    {
                        await _tableTrack.ExecuteBatchAsync(batchOperation);
                        batchOperation.Clear();
                    }

                }
                if (batchOperation.Count != 0)
                    await _tableTrack.ExecuteBatchAsync(batchOperation);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }           
        }
    }
}
