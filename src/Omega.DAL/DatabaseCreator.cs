using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Omega.DAL
{
    public class DatabaseCreator
    {
        public void CreateCleanTrackTable(string connectionString)
        {
            // Retrieve the storage  from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("CleanTrack");

            // Create the table if it doesn't exist.
            table.CreateIfNotExistsAsync();
        }

        public void CreateMoodTable(string connectionString)
        {
            // Retrieve the storage  from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("Mood");

            // Create the table if it doesn't exist.
            table.CreateIfNotExistsAsync();
        }
    }
}