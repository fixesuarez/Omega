using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Omega.DAL
{
    public class EventGroupGateway
    {
        readonly CloudStorageAccount _storageAccount;
        readonly CloudTableClient _tableClient;
        readonly CloudTable _tableEventGroup;
        readonly CloudQueueClient _queueClient;
        readonly CloudQueue _normalQueue;
        readonly CloudQueue _priorityQueue;

        public EventGroupGateway(string connectionString)
        {
            //EventGroupTable
            _storageAccount = CloudStorageAccount.Parse(connectionString);
            _tableClient = _storageAccount.CreateCloudTableClient();
            _tableEventGroup = _tableClient.GetTableReference("EventGroup");
            _tableEventGroup.CreateIfNotExistsAsync();

            _queueClient = _storageAccount.CreateCloudQueueClient();
            _normalQueue = _queueClient.GetQueueReference("normalQueue");
            _normalQueue.CreateIfNotExistsAsync();
            
            _priorityQueue = _queueClient.GetQueueReference("priorityQueue");
            _priorityQueue.CreateIfNotExistsAsync();
        }

        public async Task InsertEventGroup(string eventId, List<User> users, string type, string cover)
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            EventGroup eventGroup;

            foreach (User user in users)
            {
                EventGroup e = await RetrieveGroupEvent( eventId, user.Email );
                if( e == null )
                {
                    eventGroup = new EventGroup( eventId, user.Email );
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

        public async Task InsertNormalQueue(string email)
        {
            CloudQueueMessage message = new CloudQueueMessage(email);
            await _normalQueue.AddMessageAsync(message);
        }

        public async Task InsertPriorityQueue(string email)
        {
            CloudQueueMessage message = new CloudQueueMessage(email);
            await _priorityQueue.AddMessageAsync(message);
        }

        public async Task<TableContinuationToken> PriorityContinuationToken()
        {
            throw new NotImplementedException();
        }
    }
}
