using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

namespace Omega.DAL
{
    public class CleanTrackGateway
    {
        CloudStorageAccount _storageAccount;
        CloudTableClient _tableClient;
        CloudTable _table;
        readonly CloudQueue _queue;
        readonly CloudQueueClient _queueClient;
        public CleanTrackGateway(string connectionString)
        {
            _storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create the table client.
            _tableClient = _storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            _table = _tableClient.GetTableReference("CleanTrack");

            // Create the table if it doesn't exist.
            _table.CreateIfNotExistsAsync().Wait();

            _queueClient = _storageAccount.CreateCloudQueueClient();
            _queue = _queueClient.GetQueueReference("myqueue");
            _queue.CreateIfNotExistsAsync().Wait();
        }

        public async Task<CleanTrack> GetSongCleanTrack(string trackIdSource)
        {
            CleanTrack ct = new CleanTrack();

            TableOperation retrieveOperation = TableOperation.Retrieve<CleanTrack>(string.Empty, trackIdSource);

            TableResult retrievedResult = await _table.ExecuteAsync(retrieveOperation);

            if (retrievedResult.Result != null)
                ct = (CleanTrack)retrievedResult.Result;

            return ct;
        }

        public async Task InsertTrackQueue(string source, string trackId)
        {
            CloudQueueMessage message = new CloudQueueMessage(source + ":" + trackId);
            await _queue.AddMessageAsync(message);
        }
    }
}
