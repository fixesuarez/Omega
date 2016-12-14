using Microsoft.WindowsAzure.Storage.Table;
using Omega.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omega.DAL
{
    public class NewAmbiance : TableEntity
    {
        public NewAmbiance(string user, string name)
        {
            this.PartitionKey = user;
            this.RowKey = name;
        }

        public NewAmbiance() { }

        public string Cover { get; set; }

        public NewMetadonnees Metadonnees { get; set; }
    }
}
