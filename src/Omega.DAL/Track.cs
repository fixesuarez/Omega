using Microsoft.WindowsAzure.Storage.Table;

namespace Omega.DAL
{
    public class Track : TableEntity
    {
        public string UserId { get; set; }
        public string PlaylistId { get; set; }
        public string TrackId { get; set; }
        public string Title { get; set; }
        public string AlbumName { get; set; }
        public string Popularity { get; set; }
        public string Duration { get; set; }
        public string Cover { get; set; }

        public Track( string source, string userId, string playlistId, string trackId, string title, string albumName, string popularity, string duration, string cover )
        {
            PartitionKey = userId;
            RowKey = source + ":" + playlistId + ":" + trackId;
            UserId = userId;
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
