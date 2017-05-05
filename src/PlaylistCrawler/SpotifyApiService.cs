using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Omega.DAL;
using System;

namespace PlaylistCrawler
{
    public class SpotifyApiService
    {
        readonly TrackGateway _trackGateway;
        readonly PlaylistGateway _playlistGateway;
        readonly UserGateway _userGateway;
        readonly CleanTrackGateway _cleanTrackGateway;

        public SpotifyApiService(TrackGateway trackGateway, PlaylistGateway playlistGateway, UserGateway userGateway, CleanTrackGateway cleanTrackGateway)
        {
            _trackGateway = trackGateway;
            _playlistGateway = playlistGateway;
            _userGateway = userGateway;
            _cleanTrackGateway = cleanTrackGateway;
        }
        public async Task<SpotifyToken> TokenRefresh(string guid)
        {
            string grantType = "grant_type=refresh_token";
            string refreshToken = "refresh_token=" + await _userGateway.FindSpotifyRefreshToken(guid);
            string postString = grantType + "&" + refreshToken;

            string url = "https://accounts.spotify.com/api/token";

            using (HttpClient client = new HttpClient())
            {
                HttpRequestHeaders headers = client.DefaultRequestHeaders;
                headers.Add("Authorization", string.Format("Basic {0}", "MTQxY2NkYTBmMmQ3NDI2YTgzMzlmODk3ZDg5ZDYzNGI6Yzg4YWZjNWRkOTUyNDZjODkwNzAyMTAyMGJiODRjYjE="));
                HttpResponseMessage message = await client.PostAsync(url, new StringContent(postString, Encoding.UTF8, "application/x-www-form-urlencoded"));

                using (Stream responseStream = await message.Content.ReadAsStreamAsync())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string responseFromServer = reader.ReadToEnd();
                    var token = JsonConvert.DeserializeObject<SpotifyToken>(responseFromServer);
                    if(!string.IsNullOrEmpty(token.refresh_token))
                    {
                        await _userGateway.UpdateSpotifyRefreshToken(guid, token.refresh_token);
                    }
                    return token;
                }
            }
        }

        public async Task GetSpotifyPlaylist(string guid)
        {
            string accessToken = TokenRefresh(guid).Result.access_token;
            string pseudo = await _userGateway.RetrievePseudo(guid);
            if (!string.IsNullOrEmpty(accessToken)) {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestHeaders headers = client.DefaultRequestHeaders;
                headers.Add("Authorization", string.Format("Bearer {0}", accessToken));
                HttpResponseMessage message = await client.GetAsync("https://api.spotify.com/v1/me/playlists");

                    using (Stream responseStreamAllPlaylists = await message.Content.ReadAsStreamAsync())
                    using (StreamReader readerAllPlaylists = new StreamReader(responseStreamAllPlaylists))
                    {
                        string allplaylist;

                        string allPlaylistsString = readerAllPlaylists.ReadToEnd();

                        JObject allPlaylistsJson = JObject.Parse(allPlaylistsString);
                        JArray allPlaylistsArray = (JArray)allPlaylistsJson["items"];
                        List<Playlist> listOfPlaylists = new List<Playlist>();

                        for (int i = 0; i < allPlaylistsArray.Count; i++)
                        {
                            var playlist = allPlaylistsArray[i];

                            string requestTracksInPlaylist = (string)playlist["tracks"]["href"];

                            string idOwner = (string)playlist["owner"]["id"];
                            string name = (string)playlist["name"];
                            string idPlaylist = (string)playlist["id"];
                            string coverPlaylist = (string)playlist["images"][0]["url"];

                            Playlist p = new Playlist(idOwner, idPlaylist, await GetAllTracksInPlaylists(requestTracksInPlaylist, accessToken, idOwner, idPlaylist, coverPlaylist), name, coverPlaylist, pseudo);
                            await _playlistGateway.InsertPlaylist(p);
                            listOfPlaylists.Add(p);
                        }
                        allplaylist = JsonConvert.SerializeObject(listOfPlaylists);
                        JToken playlistsJson = JToken.Parse(allplaylist);
                    }
                }
            }
        }

        private async Task<List<Track>> GetAllTracksInPlaylists(string urlRequest, string accessToken, string userdId, string playlistId, string cover)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestHeaders headers = client.DefaultRequestHeaders;
                headers.Add("Authorization", string.Format("Bearer {0}", accessToken));
                HttpResponseMessage message = await client.GetAsync(urlRequest);

                using (Stream responseStream = await message.Content.ReadAsStreamAsync())
                using (StreamReader readerTracksInPlaylists = new StreamReader(responseStream))
                {
                    List<Track> tracksInPlaylist = new List<Track>();

                    string allTracksInPlaylistString = readerTracksInPlaylists.ReadToEnd();
                    JObject allTracksInPlaylistJson = JObject.Parse(allTracksInPlaylistString);
                    JArray allTracksInPlaylistArray = (JArray)allTracksInPlaylistJson["items"];

                    for (int j = 0; j < allTracksInPlaylistArray.Count; j++)
                    {
                        string trackIdTmp = (string)allTracksInPlaylistJson["items"][j]["track"]["id"];
                        if (await _trackGateway.RetrieveTrack("s", playlistId, trackIdTmp) == null && !string.IsNullOrEmpty(trackIdTmp) && !string.IsNullOrEmpty(playlistId))
                            await _cleanTrackGateway.InsertTrackQueue("s", trackIdTmp);
                    }

                    await _trackGateway.DeleteAllTrackPlaylist(playlistId);

                    string trackTitle = null;
                    string trackId = null;
                    string albumName = null;
                    string trackPopularity = null;
                    string duration = null;
                    string coverAlbum = null;

                    for (int i = 0; i < allTracksInPlaylistArray.Count; i++)
                    {
                        try
                        {
                            trackTitle      = (string)allTracksInPlaylistJson["items"][i]["track"]["name"];
                            trackId         = (string)allTracksInPlaylistJson["items"][i]["track"]["id"];
                            albumName       = (string)allTracksInPlaylistJson["items"][i]["track"]["album"]["name"];
                            trackPopularity = (string)allTracksInPlaylistJson["items"][i]["track"]["popularity"];
                            duration        = (string)allTracksInPlaylistJson["items"][i]["track"]["duration_ms"];
                            coverAlbum      = null;                       
                            coverAlbum = (string)allTracksInPlaylistJson["items"][i]["track"]["album"]["images"][0]["url"];
                        }
                        catch (Exception)
                        {

                        }

                        try
                        {
                            if (await _trackGateway.RetrieveTrack("s", playlistId, trackId) == null)
                                await _trackGateway.InsertTrack("s", playlistId, trackId, trackTitle, albumName, trackPopularity, duration, coverAlbum);
                            tracksInPlaylist.Add(new Track("s", playlistId, trackId, trackTitle, albumName, trackPopularity, duration, coverAlbum));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("error during spotify Tracks");
                        }
                    }
                    return tracksInPlaylist;
                }
            }
        }
    }
}
