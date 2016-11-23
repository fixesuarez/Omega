using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omega.DAL
{
    public class EventGateway
    {
        readonly CloudStorageAccount _storageAccount;
        readonly CloudTable _table;
        public EventGateway(string connectionString)
        {
             _storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create the table client.
            CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            _table = tableClient.GetTableReference("Event");

            // Create the table if it doesn't exist.
            _table.CreateIfNotExistsAsync();
        }
    }
}
