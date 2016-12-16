using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Omega.DAL
{
    public class EventGroupGateway
    {
        readonly CloudStorageAccount _storageAccount;
        readonly CloudTableClient _tableClient;
        readonly CloudTable _tableEventGroup;

        public EventGroupGateway(string connectionString)
        {
            _storageAccount = CloudStorageAccount.Parse(connectionString);
            _tableClient = _storageAccount.CreateCloudTableClient();
            _tableEventGroup = _tableClient.GetTableReference("EventGroup");
            _tableEventGroup.CreateIfNotExistsAsync();
        }

        public async Task InsertEventGroup(string eventId, List<User> users, string type, string cover)
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            EventGroup eventGroup;

            foreach (User user in users)
            {
                EventGroup e = await RetrieveGroupEvent( eventId, user.Guid );
                if( e == null )
                {
                    eventGroup = new EventGroup( eventId, user.Guid );
                    eventGroup.UserId = user.FacebookId;
                    eventGroup.Type = type;
                    eventGroup.Cover = cover;
                    batchOperation.Insert( eventGroup );
                }
            }
            if( batchOperation.Count != 0)
                await _tableEventGroup.ExecuteBatchAsync(batchOperation);
        }

        public async Task<EventGroup> RetrieveGroupEvent( string eventGroupId, string email )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<EventGroup>( eventGroupId, email );
            TableResult retrievedGroupEvent = await _tableEventGroup.ExecuteAsync( retrieveOperation );
            return (EventGroup) retrievedGroupEvent.Result;
        }
    }
}
