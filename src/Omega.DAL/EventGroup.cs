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
            StartTime = new DateTime(1900, 1, 1);
        }

        public EventGroup() { }
        public EventGroup( string groupGuid, string userGuid, string groupName )
        {
            PartitionKey = groupGuid;
            RowKey = userGuid;
            Name = groupName;
            StartTime = new DateTime();
            Type = "GroupOmega";
            Cover = string.Empty;
        }
        public EventGroup( string groupGuid, string userGuid, string eventName, DateTime startTime )
        {
            PartitionKey = groupGuid;
            RowKey = userGuid;
            Name = eventName;
            StartTime = startTime;
            Type = "EventOmega";
            Cover = string.Empty;
        }

        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public string Cover { get; set; }
    }
}
