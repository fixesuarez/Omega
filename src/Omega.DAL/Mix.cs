using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omega.DAL
{
    public class Mix : TableEntity
    {
        public Mix(string name, string userGuid)
        {
            PartitionKey = userGuid;
            RowKey = name;
        }

        public Mix() { }

        public string mood { get; set; }

        public string playlist { get; set; }
    }
}
