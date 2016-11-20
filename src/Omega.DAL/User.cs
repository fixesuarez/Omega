using Microsoft.WindowsAzure.Storage.Table;

namespace Omega.DAL
{
    public class User : TableEntity
    {
        public string Email
        {
            get { return RowKey; }
            set { RowKey = value; }
        }

        public string FacebookId { get; set; }
        public string SpotifyId { get; set; }
        public string DeezerId { get; set; }

        public string FacebookAccessToken { get; set; }

        public string DeezerAccessToken { get; set; }
        public string DeezerRefreshToken { get; set; }

        public string SpotifyAccessToken { get; set; }
        public string SpotifyRefreshToken { get; set; }

        public User() {}
        public User( string email, string facebookId, string facebookAccessToken )
        {
            PartitionKey = string.Empty;
            RowKey = email;
            FacebookId = facebookId;
            FacebookAccessToken = facebookAccessToken;
        }
        public User( string email, string spotifyId, string spotifyAccessToken, string spotifyRefreshToken )
        {
            PartitionKey = string.Empty;
            RowKey = email;
            SpotifyId = spotifyId;
            SpotifyAccessToken = spotifyAccessToken;
            SpotifyRefreshToken = spotifyRefreshToken;
        }
    }
}
