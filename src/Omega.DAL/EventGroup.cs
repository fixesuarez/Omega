using Microsoft.WindowsAzure.Storage.Table;
using System;

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
            StartTime = new DateTime(1900, 1, 1);
            Type = "groupOmega";
            Cover = string.Empty;
        }
        public EventGroup( string groupGuid, string userGuid, string eventName, DateTime startTime, string location, string cover )
        {
            PartitionKey = groupGuid;
            RowKey = userGuid;
            Name = eventName;
            StartTime = startTime;
            Type = "eventOmega";
            Cover = cover;
            Location = location;
        }

        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public string Cover { get; set; }
        public string Location { get; set; }
        public bool Owner { get; set; }
        public string Members { get; set; }
    }
}
