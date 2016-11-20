using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;

namespace Omega.DAL
{
    public class Playlist : TableEntity
    {
        public string OwnerId { get; set; }
        public string PlaylistId { get; set; }
        public List<Track> Tracks { get; set; }
        public string Name { get; set; }
        public string Cover { get; set; }

        public Playlist() { }
    }
}
