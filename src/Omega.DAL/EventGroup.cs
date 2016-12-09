using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omega.DAL
{
    public class EventGroup : TableEntity
    {
        public EventGroup(string eventId, string userEmail)
        {
            PartitionKey = eventId;
            RowKey = userEmail;
        }

        public EventGroup() { }

        public string UserId { get; set; }

        public string Type { get; set; }

        public string Cover { get; set; }
    }
}
