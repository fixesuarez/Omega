using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omega.DAL
{
    public class EventGroup : TableEntity
    {
        public EventGroup(string eventId, string userGuid)
        {
            PartitionKey = eventId;
            RowKey = userGuid;
        }

        public EventGroup() { }

        public string Name { get; set; }

        public DateTime StartTime { get; set; }

        public string UserId { get; set; }

        public string Type { get; set; }

        public string Cover { get; set; }
    }
}
