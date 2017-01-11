using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

namespace Omega.DAL
{
    public class CleanTrackGateway
    {
        CloudStorageAccount _storageAccount;
        CloudTableClient _tableClient;
        CloudTable _table;
        public CleanTrackGateway(string connectionString)
        {
            _storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create the table client.
            _tableClient = _storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            _table = _tableClient.GetTableReference("CleanTrack");

            // Create the table if it doesn't exist.
            _table.CreateIfNotExistsAsync().Wait();
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
    }
}
