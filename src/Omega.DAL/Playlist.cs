using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;

namespace Omega.DAL
{
    public class Playlist : TableEntity
    {
        public string OwnerId
        {
            get
            {
                return PartitionKey;
            }
            set
            {
                PartitionKey = value;
            }
        }
        public string PlaylistId
        {
            get
            {
                return RowKey;
            }
            set
            {
                RowKey = value;
            }
        }
        public List<Track> Tracks { get; set; }
        public string Name { get; set; }
        public string Cover { get; set; }
        public string Pseudo { get; set; }

        public Playlist() { }
        public Playlist( string ownerId, string playlistId, List<Track> tracks, string name, string cover, string pseudoOwner )
        {
            PartitionKey = ownerId;
            RowKey = playlistId;
            Tracks = tracks;
            Name = name;
            Cover = cover;
            Pseudo = pseudoOwner;
        }
    }
}
