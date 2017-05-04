using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Omega.DAL
{
    public class User : TableEntity
    {
        public string Guid
        {
            get { return RowKey; }
            set { RowKey = value; }
        }

        public string FacebookEmail { get; set; }
        public string SpotifyEmail { get; set; }
        public string DeezerEmail { get; set; }

        public string FacebookId { get; set; }
        public string SpotifyId { get; set; }
        public string DeezerId { get; set; }

        public string FacebookAccessToken { get; set; }

        public string DeezerAccessToken { get; set; }
        //public string DeezerRefreshToken { get; set; }

        public string SpotifyAccessToken { get; set; }
        public string SpotifyRefreshToken { get; set; }

        public string EventsId { get; set; }
        public string GroupsId { get; set; }

        public string Pseudo { get; set; }
        public string FacebookName { get; set; }

        public User() {}
    }
}
