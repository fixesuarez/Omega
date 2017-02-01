using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.AspNetCore.Http;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Omega.DAL
{
    public class EventGroupGateway
    {
        readonly EventGroupUserGateway _eventGroupUserGateway;
        readonly CloudStorageAccount _storageAccount;
        readonly CloudTableClient _tableClient;
        readonly CloudTable _tableEventGroup;
        readonly CloudQueueClient _queueClient;
        readonly CloudQueue _normalQueue;
        readonly CloudQueue _priorityQueue;

        readonly CloudBlobClient _blobClient;
        readonly CloudBlobContainer _container;
        CloudBlockBlob _blockBlob;

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

            // Create the blob client.
            _blobClient = _storageAccount.CreateCloudBlobClient();
            // Retrieve reference to a previously created container.
            _container = _blobClient.GetContainerReference( "images-eventgroup" );
            // Create the container if it doesn't already exist.
            _container.CreateIfNotExistsAsync().Wait();

            _eventGroupUserGateway = new EventGroupUserGateway(connectionString);
        }

        public async Task CreateEventOmega( string eventGuid, string userGuid, string eventName, DateTime startTime, string location )
        {
            EventGroup eventOmega = new EventGroup( eventGuid, userGuid, eventName, startTime, location );
            eventOmega.Owner = true;
            TableOperation insertEventOmegaOperation = TableOperation.Insert( eventOmega );
            await _tableEventGroup.ExecuteAsync( insertEventOmegaOperation );
            await _eventGroupUserGateway.InsertEventGroup(eventOmega);
        }
        public async Task CreateGroupOmega( string groupGuid, string userGuid, string groupName, string ownerPseudo )
        {
            List<string> members = new List<string>();
            if(ownerPseudo != null)
                members.Add(ownerPseudo);
            EventGroup groupOmega = new EventGroup( groupGuid, userGuid, groupName );
            groupOmega.Owner = true;
            groupOmega.Members = JsonConvert.SerializeObject(members);
            TableOperation insertGroupOmegaOperation = TableOperation.Insert( groupOmega );
            await _tableEventGroup.ExecuteAsync( insertGroupOmegaOperation );
            await _eventGroupUserGateway.InsertEventGroup(groupOmega);
        }

        public async Task<bool> IsUserEventGroupOwner( string eventGroupId, string userGuid )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<EventGroup>( eventGroupId, userGuid );
            TableResult retrievedResult = await _tableEventGroup.ExecuteAsync( retrieveOperation );
            EventGroup retrievedEventGroup = (EventGroup) retrievedResult.Result;
            if( retrievedEventGroup != null )
                return retrievedEventGroup.Owner;
            else
                return false;
        }
        public async Task<string> GetEventGroupName( string eventGroupId, string userGuid )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<EventGroup>( eventGroupId, userGuid );
            TableResult retrievedResult = await _tableEventGroup.ExecuteAsync( retrieveOperation );
            EventGroup retrievedEventGroup = (EventGroup) retrievedResult.Result;
            if( retrievedEventGroup != null )
                return retrievedEventGroup.Name;
            else
                return string.Empty;
        }

        public async Task UploadEventGroupCover( IFormFile eventGroupCover, string eventGroupGuid, string eventGroupName )
        {
            _blockBlob = _container.GetBlockBlobReference( eventGroupGuid + ":" + eventGroupName );
            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            if( eventGroupCover.Length > 0 )
            {
                using( var stream = new FileStream( filePath, FileMode.Create ) )
                {
                    await eventGroupCover.CopyToAsync( stream );
                }
                using( var fileStream = File.OpenRead( filePath ) )
                {
                    await _blockBlob.UploadFromStreamAsync( fileStream );
                }
            }
        }
        public async Task DeleteBlobEventGroupCover( string eventGroupId, string eventGroupName )
        {
            CloudBlockBlob blockBlob = _container.GetBlockBlobReference( eventGroupId + ":" + eventGroupName );
            await blockBlob.DeleteAsync();
        }

        public async Task AddMemberToEventGroupOmega( EventGroup eventGroupOmega )
        {
            eventGroupOmega.Owner = false;
            TableOperation insertEventGroupOmegaOperation = TableOperation.Insert( eventGroupOmega );
            await _tableEventGroup.ExecuteAsync( insertEventGroupOmegaOperation );
            await _eventGroupUserGateway.AddMemberToEventGroupOmega( eventGroupOmega );
        }

        public async Task<EventGroup> FindEventGroup( string idEventGroup, string guidUser )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<EventGroup>( idEventGroup, guidUser );
            TableResult retrievedResult = await _tableEventGroup.ExecuteAsync( retrieveOperation );
            return (EventGroup) retrievedResult.Result;
        }

        public async Task DeleteEventGroupOmega(string eventId, string userGuid )
        {
            List<EventGroup> eventGroups = new List<EventGroup>();
            TableQuery<EventGroup> query = new TableQuery<EventGroup>().
                Where( TableQuery.GenerateFilterCondition( "PartitionKey", QueryComparisons.Equal, eventId ) );
            query.TakeCount = 1000;
            TableContinuationToken tableContinuationToken = null;
            do
            {
                var queryResponse = await _tableEventGroup.ExecuteQuerySegmentedAsync( query, tableContinuationToken );
                tableContinuationToken = queryResponse.ContinuationToken;
                eventGroups.AddRange( queryResponse.Results );
            } while( tableContinuationToken != null );

            TableBatchOperation batchOperation = new TableBatchOperation();
            foreach( EventGroup eventGroup in eventGroups )
            {
                batchOperation.Delete( eventGroup );
            }
            await _tableEventGroup.ExecuteBatchAsync( batchOperation );
        }

        public async Task DeleteOneEventGroupOmega(string eventId, string userGuid)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<EventGroup>(eventId, userGuid);
            TableResult retrievedResult = await _tableEventGroup.ExecuteAsync(retrieveOperation);
            EventGroup deleteEntity = (EventGroup)retrievedResult.Result;
            if (deleteEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                await _tableEventGroup.ExecuteAsync(deleteOperation);
            }
        }

        public async Task InsertEventGroup(string eventId, List<User> users, string type, string cover, string name, DateTime startTime, string location)
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
                    eventGroup.Location = location;
                    batchOperation.Insert( eventGroup );
                }
                else
                {
                    await UpdateEventGroup(eventId, user, type, cover, name, startTime, location);
                }
            }
            if( batchOperation.Count != 0)
                await _tableEventGroup.ExecuteBatchAsync(batchOperation);
            await _eventGroupUserGateway.InsertBatchEventGroup(eventId, users, type, cover, name, startTime, location);
        }
        public async Task InsertEventGroup(string eventId, List<User> users, string type, string cover, string name, JArray pmembers)
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            
            EventGroup eventGroup;
            List<string> members = new List<string>();
            foreach (var user in pmembers)
            {
                members.Add((string)user["name"]);
            }
            foreach (User user in users)
            {
                eventGroup = await RetrieveGroupEvent(eventId, user.RowKey);
                if (eventGroup == null)
                {
                    eventGroup = new EventGroup(eventId, user.RowKey);
                    eventGroup.UserId = user.FacebookId;
                    eventGroup.Type = type;
                    eventGroup.Cover = cover;
                    eventGroup.Name = name;
                    eventGroup.Members = JsonConvert.SerializeObject(members);
                    batchOperation.Insert(eventGroup);
                    TableOperation insert = TableOperation.Insert(eventGroup);
                    await _tableEventGroup.ExecuteAsync(insert);
                }
                else
                {
                    await UpdateEventGroup(eventId, user, type, cover, name, members);
                }
            }
            await _eventGroupUserGateway.InsertBatchEventGroup(eventId, users, type, cover, name, members);
        }

        public async Task UpdateEventGroup(string eventId, User user, string type, string cover, string name, DateTime startTime, string location)
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
                updateEntity.Location = location;

                TableOperation updateOperation = TableOperation.Replace(updateEntity);

                await _tableEventGroup.ExecuteAsync(updateOperation);
            }
        }
        public async Task UpdateEventGroup(string eventId, User user, string type, string cover, string name, List<string> users)
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
                updateEntity.Members = JsonConvert.SerializeObject(users);

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
