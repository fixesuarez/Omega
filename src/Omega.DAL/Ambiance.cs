using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omega.DAL
{
    public class Ambiance : TableEntity
    {
        public Ambiance(string user, string name)
        {
            this.PartitionKey = user;
            this.RowKey = name;
        }

        public Ambiance() { }

        public string Cover { get; set; }

        public string Danceability { get; set; }

        public string Energy { get; set; }

        public string Loudness { get; set; }

        public string Mode { get; set; }

        public string Speechiness { get; set; }

        public string Accousticness { get; set; }

        public string Instrumentalness { get; set; }

        public string Liveness { get; set; }

        public string Valence { get; set; }

        public string Tempo { get; set; }

        public string Popularity { get; set; }
    }
}