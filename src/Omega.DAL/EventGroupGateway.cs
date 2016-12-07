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
        private EventGroup _eventGroup;

        public EventGroupGateway(string connectionString)
        {
            _storageAccount = CloudStorageAccount.Parse(connectionString);
            _tableClient = _storageAccount.CreateCloudTableClient();
            _tableEventGroup = _tableClient.GetTableReference("EventGroup");
            _tableEventGroup.CreateIfNotExistsAsync();
        }

        public async Task InsertEvent(string eventId, List<User> users, string type, string cover)
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            foreach (User _user in users)
            {
                _eventGroup = new EventGroup(eventId, _user.Email);
                _eventGroup.Id = _user.FacebookId;
                _eventGroup.Type = type;
                _eventGroup.Cover = cover;
                batchOperation.Insert(_eventGroup);
            }
            await _tableEventGroup.ExecuteBatchAsync(batchOperation);
        }
    }
}
