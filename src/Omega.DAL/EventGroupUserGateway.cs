using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omega.DAL
{
    public class EventGroupUserGateway
    {
        readonly CloudStorageAccount _storageAccount;
        readonly CloudTableClient _tableClient;
        readonly CloudTable _tableEventGroup;

        readonly CloudBlobClient _blobClient;
        readonly CloudBlobContainer _container;
        public EventGroupUserGateway(string connectionString)
        {
            //EventGroupTable
            _storageAccount = CloudStorageAccount.Parse(connectionString);
            _tableClient = _storageAccount.CreateCloudTableClient();
            _tableEventGroup = _tableClient.GetTableReference("EventGroupUser");
            _tableEventGroup.CreateIfNotExistsAsync().Wait();

            // Create the blob client.
            _blobClient = _storageAccount.CreateCloudBlobClient();
            // Retrieve reference to a previously created container.
            _container = _blobClient.GetContainerReference("images-eventgroup");
            // Create the container if it doesn't already exist.
            _container.CreateIfNotExistsAsync().Wait();
        }

        public async Task InsertEventGroup(EventGroup pEvent)
        {
            EventGroupUser eventUser = new EventGroupUser(pEvent.PartitionKey, pEvent.RowKey);
            eventUser.StartTime = pEvent.StartTime;
            eventUser.Owner = pEvent.Owner;
            eventUser.Location = pEvent.Location;
            eventUser.Name = pEvent.Name;
            eventUser.Type = pEvent.Type;
            eventUser.UserId = pEvent.UserId;
            //eventUser.Cover = pEvent.Cover;
            eventUser.Members = pEvent.Members;

            if(await RetrieveGroupEvent(pEvent.PartitionKey, pEvent.RowKey) == null)
            {
                TableOperation insert = TableOperation.Insert(eventUser);
                await _tableEventGroup.ExecuteAsync(insert);
            }
        }

        public async Task AddMemberToEventGroupOmega( EventGroup pEvent)
        {
            pEvent.Owner = false;
            EventGroupUser eventUser = new EventGroupUser(pEvent.PartitionKey, pEvent.RowKey);
            eventUser.StartTime = pEvent.StartTime;
            eventUser.Owner = pEvent.Owner;
            eventUser.Location = pEvent.Location;
            eventUser.Name = pEvent.Name;
            eventUser.Type = pEvent.Type;
            eventUser.UserId = pEvent.UserId;
            //eventUser.Cover = pEvent.Cover;
            eventUser.Members = pEvent.Members;
            TableOperation insertEventGroupOmegaOperation = TableOperation.Insert(eventUser);
            await _tableEventGroup.ExecuteAsync( insertEventGroupOmegaOperation );
        }

        public async Task InsertBatchEventGroup(string eventId, List<User> users, string type, string cover, string name, List<string> pmembers)
        {
            TableBatchOperation batchOperation = new TableBatchOperation();

            EventGroupUser eventGroup;

            foreach (User user in users)
            {
                eventGroup = await RetrieveGroupEvent(eventId, user.RowKey);
                if (eventGroup == null)
                {
                    eventGroup = new EventGroupUser(eventId, user.RowKey);
                    eventGroup.UserId = user.FacebookId;
                    eventGroup.Type = type;
                    eventGroup.Cover = cover;
                    eventGroup.Name = name;
                    eventGroup.Members = JsonConvert.SerializeObject(pmembers);
                    TableOperation insert = TableOperation.Insert(eventGroup);
                    await _tableEventGroup.ExecuteAsync(insert);
                }
                else
                {
                    await UpdateEventGroup(eventId, user, type, cover, name, pmembers);
                }
            }
        }

        public async Task InsertBatchEventGroup(string eventId, List<User> users, string type, string cover, string name, DateTime startTime, string location)
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            EventGroupUser eventGroup;

            foreach (User user in users)
            {
                EventGroupUser e = await RetrieveGroupEvent(eventId, user.RowKey);
                if (e == null)
                {
                    eventGroup = new EventGroupUser(eventId, user.RowKey);
                    eventGroup.UserId = user.FacebookId;
                    eventGroup.Type = type;
                    eventGroup.Cover = cover;
                    eventGroup.Name = name;
                    eventGroup.StartTime = startTime;
                    eventGroup.Location = location;
                    eventGroup.Members = JsonConvert.SerializeObject(users);
                    batchOperation.Insert(eventGroup);
                }
                else
                {
                    await UpdateEventGroup(eventId, user, type, cover, name, startTime, location);
                }
            }
            if (batchOperation.Count != 0)
                await _tableEventGroup.ExecuteBatchAsync(batchOperation);
        }

        public async Task UpdateEventGroup(string eventId, User user, string type, string cover, string name, DateTime startTime, string location)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<EventGroupUser>(user.RowKey, eventId);
            TableResult retrievedResult = await _tableEventGroup.ExecuteAsync(retrieveOperation);
            EventGroupUser updateEntity = (EventGroupUser)retrievedResult.Result;

            if (updateEntity != null)
            {
                EventGroupUser track = new EventGroupUser(eventId, user.RowKey);
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
        public async Task UpdateEventGroup(string eventId, User user, string type, string cover, string name, List<string> members)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<EventGroupUser>(user.RowKey, eventId);
            TableResult retrievedResult = await _tableEventGroup.ExecuteAsync(retrieveOperation);
            EventGroupUser updateEntity = (EventGroupUser)retrievedResult.Result;

            if (updateEntity != null)
            {
                EventGroupUser track = new EventGroupUser(eventId, user.RowKey);
                updateEntity.UserId = user.FacebookId;
                updateEntity.Type = type;
                updateEntity.Cover = cover;
                updateEntity.Name = name;
                updateEntity.Members = JsonConvert.SerializeObject(members);

                TableOperation updateOperation = TableOperation.Replace(updateEntity);

                await _tableEventGroup.ExecuteAsync(updateOperation);
            }
        }

        public async Task<EventGroupUser> RetrieveGroupEvent(string eventGroupId, string guid)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<EventGroupUser>(guid, eventGroupId);
            TableResult retrievedGroupEvent = await _tableEventGroup.ExecuteAsync(retrieveOperation);
            return (EventGroupUser)retrievedGroupEvent.Result;
        }

        public async Task<string> GetAllEventsUser(string guid, string type)
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            List<EventGroupUser> tracks = new List<EventGroupUser>();
            List<EventGroupUser> tracksDef = new List<EventGroupUser>();
            TableQuery<EventGroupUser> query = new TableQuery<EventGroupUser>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, guid));

            query.TakeCount = 1000;
            TableContinuationToken tableContinuationToken = null;
            do
            {
                var queryResponse = await _tableEventGroup.ExecuteQuerySegmentedAsync(query, tableContinuationToken);
                tableContinuationToken = queryResponse.ContinuationToken;
                tracks.AddRange(queryResponse.Results);
            } while (tableContinuationToken != null);
            foreach(EventGroupUser pEvent in tracks)
            {
                if (pEvent.Type.Contains(type))
                {
                    if (pEvent.Type.Contains("event") && (DateTime.Compare(pEvent.StartTime, DateTime.Now) > 0 || DateTime.Compare(pEvent.StartTime, DateTime.Now) == 0))
                    {                      
                        tracksDef.Add(pEvent);
                    }
                    else if(pEvent.Type.Contains("group"))
                    {
                        if(pEvent.Members != null)
                            pEvent.ListMembers = JsonConvert.DeserializeObject<List<string>>(pEvent.Members);
                        tracksDef.Add(pEvent);
                    }
                }
            }
            return JsonConvert.SerializeObject(tracksDef);
        }
    }
}
