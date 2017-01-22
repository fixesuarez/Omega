using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Omega.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PlaylistCrawler
{
    public class DeezerApiService
    {
        readonly TrackGateway _trackGateway;
        readonly PlaylistGateway _playlistGateway;
        readonly UserGateway _userGateway;
        readonly CleanTrackGateway _cleanTrackGateway;
        public DeezerApiService(TrackGateway trackGateway, PlaylistGateway playlistGateway, UserGateway userGateway, CleanTrackGateway cleanTrackGateway)
        {
            _trackGateway = trackGateway;
            _playlistGateway = playlistGateway;
            _userGateway = userGateway;
            _cleanTrackGateway = cleanTrackGateway;
        }
        public async Task GetAllDeezerPlaylists(string guid)
        {
            using (HttpClient client = new HttpClient())
            {
                string accessToken = await _userGateway.FindDeezerAccessToken(guid);
                string pseudo = await _userGateway.RetrievePseudo(guid);

                Uri allPlaylistsUri = new Uri(string.Format(
                "http://api.deezer.com/user/me/playlists?output=json&access_token={0}",
                accessToken));

                HttpResponseMessage message = await client.GetAsync(allPlaylistsUri);

                using (Stream responseStream = await message.Content.ReadAsStreamAsync())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    JObject allPlaylistsJson;
                    string allplaylist;

                    string allPlaylistsString = reader.ReadToEnd();

                        allPlaylistsJson = JObject.Parse(allPlaylistsString);
                        JArray allPlaylistsArray = (JArray)allPlaylistsJson["data"];
                    if (allPlaylistsArray != null)
                    {
                        List<Playlist> listOfPlaylists = new List<Playlist>();

                        for (int i = 0; i < allPlaylistsArray.Count; i++)
                        {
                            var playlist = allPlaylistsArray[i];

                            string requestTracksInPlaylist = (string)playlist["tracklist"];
                            requestTracksInPlaylist = requestTracksInPlaylist.Replace("https", "http");

                            string idOwner = (string)playlist["creator"]["id"];
                            string name = (string)playlist["title"];
                            string idPlaylist = (string)playlist["id"];
                            string coverPlaylist = (string)playlist["picture"];
                            string pseudoOwner = (string) playlist["creator"]["name"];

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
                List<Track> tracksInPlaylist = new List<Track>();
                Uri tracksInPlaylistUri = new Uri(String.Format(
                    "{0}?output=json&access_token={1}",
                    urlRequest,
                    accessToken));
                HttpResponseMessage message = await client.GetAsync(tracksInPlaylistUri);

                using (Stream responseStream = await message.Content.ReadAsStreamAsync())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string allTracksInPlaylistString = reader.ReadToEnd();
                    JObject allTracksInPlaylistJson = JObject.Parse(allTracksInPlaylistString);
                    JArray allTracksInPlaylistArray = (JArray)allTracksInPlaylistJson["data"];

                    for (int j = 0; j < allTracksInPlaylistArray.Count; j++)
                    {
                        var trackTmp = allTracksInPlaylistArray[j];
                        string trackIdTmp = (string)trackTmp["id"];
                        if (await _trackGateway.RetrieveTrack("d", playlistId, trackIdTmp) == null)
                            await _cleanTrackGateway.InsertTrackQueue("d", trackIdTmp);
                    }

                    await _trackGateway.DeleteAllTrackPlaylist(playlistId);

                    for (int i = 0; i < allTracksInPlaylistArray.Count; i++)
                    {
                        var track = allTracksInPlaylistArray[i];

                        string trackTitle = (string)track["title"];
                        string trackId = (string)track["id"];
                        string albumName = (string)track["album"]["title"];
                        string trackRank = (string)track["rank"];
                        string duration = (string)track["duration"];
                        string coverAlbum = (string)track["album"]["cover"];

                        if (await _trackGateway.RetrieveTrack("d", playlistId, trackId) == null)
                            await _trackGateway.InsertTrack("d", playlistId, trackId, trackTitle, albumName, trackRank, duration, coverAlbum);
                        tracksInPlaylist.Add(new Track("d", playlistId, trackId, trackTitle, albumName, trackRank, duration, coverAlbum));
                        //if (await _cleanTrackGateway.GetSongCleanTrack("d:" + trackId) == null)
                        //    await _cleanTrackGateway.InsertTrackQueue("d", trackId);
                    }
                }
                return tracksInPlaylist;
            }
        }
    }
}
