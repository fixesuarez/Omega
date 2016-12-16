﻿using Microsoft.WindowsAzure.Storage;
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
                else
                {
                    await UpdateEventGroup(eventId, user, type, cover);
                }
            }
            if( batchOperation.Count != 0)
                await _tableEventGroup.ExecuteBatchAsync(batchOperation);
        }

        public async Task UpdateEventGroup(string eventId, User user, string type, string cover)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<EventGroup>(eventId, user.Email);
            TableResult retrievedResult = await _tableEventGroup.ExecuteAsync(retrieveOperation);
            EventGroup updateEntity = (EventGroup)retrievedResult.Result;

            if (updateEntity != null)
            {
                EventGroup track = new EventGroup(eventId, user.Email);
                updateEntity.UserId = user.FacebookId;
                updateEntity.Type = type;
                updateEntity.Cover = cover;

                TableOperation updateOperation = TableOperation.Replace(updateEntity);

                await _tableEventGroup.ExecuteAsync(updateOperation);
            }
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
