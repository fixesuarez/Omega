using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;

namespace Omega.DAL
{
    public class EventGroupUser : TableEntity
    {
        public EventGroupUser(string eventId, string userGuid)
        {
            PartitionKey = userGuid;
            RowKey = eventId;
            StartTime = new DateTime(1900, 1, 1);
        }

        public EventGroupUser() { }
        public EventGroupUser(string groupGuid, string userGuid, string groupName)
        {
            PartitionKey = groupGuid;
            RowKey = userGuid;
            Name = groupName;
            StartTime = new DateTime(1900, 1, 1);
            Type = "GroupOmega";
            Cover = string.Empty;
        }
        public EventGroupUser(string groupGuid, string userGuid, string eventName, DateTime startTime, string location, string cover)
        {
            PartitionKey = groupGuid;
            RowKey = userGuid;
            Name = eventName;
            StartTime = startTime;
            Type = "EventOmega";
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
        public List<string> ListMembers { get; set; }
    }
}
