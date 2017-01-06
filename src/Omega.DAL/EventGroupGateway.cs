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
            _tableEventGroup.CreateIfNotExistsAsync().Wait();

            _queueClient = _storageAccount.CreateCloudQueueClient();
            _normalQueue = _queueClient.GetQueueReference("normalqueue");
            _normalQueue.CreateIfNotExistsAsync().Wait();
            
            _priorityQueue = _queueClient.GetQueueReference("priorityqueue");
            _priorityQueue.CreateIfNotExistsAsync().Wait();
        }

        public async Task InsertEventGroup(string eventId, List<User> users, string type, string cover, string name, DateTime startTime)
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            EventGroup eventGroup;

            foreach (User user in users)
            {
                EventGroup e = await RetrieveGroupEvent( eventId, user.RowKey);
                if( e == null )
                {
                    eventGroup = new EventGroup( eventId, user.RowKey );
                    eventGroup.UserId = user.FacebookId;
                    eventGroup.Type = type;
                    eventGroup.Cover = cover;
                    eventGroup.Name = name;
                    eventGroup.StartTime = startTime;
                    batchOperation.Insert( eventGroup );
                }
                else
                {
                    await UpdateEventGroup(eventId, user, type, cover, name, startTime);
                }
            }
            if( batchOperation.Count != 0)
                await _tableEventGroup.ExecuteBatchAsync(batchOperation);
        }

        public async Task InsertEventGroup(string eventId, List<User> users, string type, string cover, string name)
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            EventGroup eventGroup;

            foreach (User user in users)
            {
                EventGroup e = await RetrieveGroupEvent(eventId, user.RowKey);
                if (e == null)
                {
                    eventGroup = new EventGroup(eventId, user.RowKey);
                    eventGroup.UserId = user.FacebookId;
                    eventGroup.Type = type;
                    eventGroup.Cover = cover;
                    eventGroup.Name = name;
                    batchOperation.Insert(eventGroup);
                }
                else
                {
                    await UpdateEventGroup(eventId, user, type, cover, name);
                }
            }
            if (batchOperation.Count != 0)
                await _tableEventGroup.ExecuteBatchAsync(batchOperation);
        }

        public async Task UpdateEventGroup(string eventId, User user, string type, string cover, string name, DateTime startTime)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<EventGroup>(eventId, user.RowKey);
            TableResult retrievedResult = await _tableEventGroup.ExecuteAsync(retrieveOperation);
            EventGroup updateEntity = (EventGroup)retrievedResult.Result;

            if (updateEntity != null)
            {
                EventGroup track = new EventGroup(eventId, user.RowKey);
                updateEntity.UserId = user.FacebookId;
                updateEntity.Type = type;
                updateEntity.Cover = cover;
                updateEntity.Name = name;
                updateEntity.StartTime = startTime;

                TableOperation updateOperation = TableOperation.Replace(updateEntity);

                await _tableEventGroup.ExecuteAsync(updateOperation);
            }
        }

        public async Task UpdateEventGroup(string eventId, User user, string type, string cover, string name)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<EventGroup>(eventId, user.RowKey);
            TableResult retrievedResult = await _tableEventGroup.ExecuteAsync(retrieveOperation);
            EventGroup updateEntity = (EventGroup)retrievedResult.Result;

            if (updateEntity != null)
            {
                EventGroup track = new EventGroup(eventId, user.RowKey);
                updateEntity.UserId = user.FacebookId;
                updateEntity.Type = type;
                updateEntity.Cover = cover;
                updateEntity.Name = name;

                TableOperation updateOperation = TableOperation.Replace(updateEntity);

                await _tableEventGroup.ExecuteAsync(updateOperation);
            }
        }

        public async Task<EventGroup> RetrieveGroupEvent( string eventGroupId, string guid )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<EventGroup>( eventGroupId, guid );
            TableResult retrievedGroupEvent = await _tableEventGroup.ExecuteAsync( retrieveOperation );
            return (EventGroup) retrievedGroupEvent.Result;
        }

        public async Task<List<EventGroup>> RetrieveMembersFromGroupEvent(string idEventGroup )
        {
            List<EventGroup> membersFromEventGroup = new List<EventGroup>();
            TableQuery<EventGroup> query = new TableQuery<EventGroup>()
                .Where(TableQuery.GenerateFilterCondition( "PartitionKey", QueryComparisons.Equal, idEventGroup ));

            query.TakeCount = 1000;
            TableContinuationToken tableContinuationToken = null;
            do
            {
                var queryResponse = await _tableEventGroup.ExecuteQuerySegmentedAsync( query, tableContinuationToken );
                tableContinuationToken = queryResponse.ContinuationToken;
                membersFromEventGroup.AddRange( queryResponse.Results );
            } while ( tableContinuationToken != null );

            return membersFromEventGroup;
        }
        public async Task InsertNormalQueue(string guid)
        {
            CloudQueueMessage message = new CloudQueueMessage(guid);
            await _normalQueue.AddMessageAsync(message);
        }

        public async Task InsertPriorityQueue(string guid)
        {
            CloudQueueMessage message = new CloudQueueMessage(guid);
            await _priorityQueue.AddMessageAsync(message);
        }

        public async Task<TableQuerySegment<EventGroup>> TableQueryResult()
        {
            TableContinuationToken continuationToken = null;
            TableQuery<EventGroup> tableQuery = new TableQuery<EventGroup>();
            TableQuerySegment<EventGroup> tableQueryResult;
            return tableQueryResult = await _tableEventGroup.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);
        }

        public async Task DeleteMessagePriorityQueue(CloudQueueMessage message)
        {
            await _priorityQueue.DeleteMessageAsync(message);
        }

        public async Task DeleteMessageNormalQueue(CloudQueueMessage message)
        {
            await _normalQueue.DeleteMessageAsync(message);
        }

        public async Task<CloudQueueMessage> GetMessagePriorityQueue()
        {
            return await _priorityQueue.GetMessageAsync();
        }
        public async Task<CloudQueueMessage> GetMessageNormalQueue()
        {
            return await _normalQueue.GetMessageAsync();
        }
    }
}
