using Microsoft.WindowsAzure.Storage.Table;

namespace Omega.DAL
{
    public class Track : TableEntity
    {
        public string PlaylistId {
            get
            {
                return PartitionKey;
            }
            set
            {
                PartitionKey = value;
            }
        }
        public string TrackId { get; set; }
        public string Title { get; set; }
        public string AlbumName { get; set; }
        public string Popularity { get; set; }
        public string Duration { get; set; }
        public string Cover { get; set; }
        public string DeezerId { get; set; }

        public Track( string source, string playlistId, string trackId, string title, string albumName, string popularity, string duration, string cover )
        {
            PartitionKey = playlistId;
            RowKey = source + ":" + playlistId + ":" + trackId;
            PlaylistId = playlistId;
            TrackId = trackId;
            Title = title;
            AlbumName = albumName;
            Popularity = popularity;
            Duration = duration;
            Cover = cover;
        }

        public Track() { }
    }
}
