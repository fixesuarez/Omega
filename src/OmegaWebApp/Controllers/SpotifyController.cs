using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Newtonsoft.Json.Linq;
using Omega.DAL;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using OmegaWebApp.Services;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OmegaWebApp.Controllers
{
    [Route( "api/[controller]" )]
    public class SpotifyController : Controller
    {
        readonly PlaylistService _playlistService;
        readonly TrackService _trackService;
        readonly UserService _userService;

        public SpotifyController( PlaylistService playlistService, TrackService trackService, UserService userService )
        {
            _playlistService = playlistService;
            _trackService = trackService;
            _userService = userService;
        }

        /// <summary>
        /// Insert the tracks of the current playlist in the table Track if they aren't in already.
        /// </summary>
        /// <param name="urlRequest"></param>
        /// <param name="accessToken"></param>
        /// <param name="userdId"></param>
        /// <param name="playlistId"></param>
        /// <returns>Returns all the tracks of the playlist.</returns>
        private async Task<List<Track>> GetAllTracksInPlaylists( string urlRequest, string accessToken, string userdId, string playlistId, string cover )
        {
            using( HttpClient client = new HttpClient() )
            {
                HttpRequestHeaders headers = client.DefaultRequestHeaders;
                headers.Add( "Authorization", string.Format( "Bearer {0}", accessToken ) );
                HttpResponseMessage message = await client.GetAsync( urlRequest );

                using( Stream responseStream = await message.Content.ReadAsStreamAsync() )
                using( StreamReader readerTracksInPlaylists = new StreamReader( responseStream ) )
                {
                    List<Track> tracksInPlaylist = new List<Track>();

                    string allTracksInPlaylistString = readerTracksInPlaylists.ReadToEnd();
                    JObject allTracksInPlaylistJson = JObject.Parse( allTracksInPlaylistString );
                    JArray allTracksInPlaylistArray = (JArray) allTracksInPlaylistJson["items"];

                    for( int i = 0; i < allTracksInPlaylistArray.Count; i++ )
                    {
                        string trackTitle = (string) allTracksInPlaylistJson["items"][i]["track"]["name"];
                        string trackId = (string) allTracksInPlaylistJson["items"][i]["track"]["id"];
                        string albumName = (string) allTracksInPlaylistJson["items"][i]["track"]["album"]["name"];
                        string trackPopularity = (string) allTracksInPlaylistJson["items"][i]["track"]["popularity"];
                        string duration = (string) allTracksInPlaylistJson["items"][i]["track"]["duration_ms"];
                        string coverAlbum = (string) allTracksInPlaylistJson["items"][i]["track"]["album"]["images"][0]["url"];

                        if( await _trackService.GetTrack( "s", playlistId, trackId ) == null )
                            await _trackService.InsertTrack( "s", playlistId, trackId, trackTitle, albumName, trackPopularity, duration, coverAlbum );
                        tracksInPlaylist.Add( new Track( "s", playlistId, trackId, trackTitle, albumName, trackPopularity, duration, coverAlbum ) );
                    }
                    return tracksInPlaylist;
                }
            }
        }

        [HttpGet( "Playlists" )]
        public async Task<JToken> GetAllSpotifyPlaylists()
        {
            string email = User.FindFirst( ClaimTypes.Email ).Value;
            string accessToken = _userService.GetSpotifyAccessToken( email ).Result;
            
            using( HttpClient client = new HttpClient() )
            {
                HttpRequestHeaders headers = client.DefaultRequestHeaders;
                headers.Add( "Authorization", string.Format( "Bearer {0}", accessToken ) );
                HttpResponseMessage message = await client.GetAsync( "https://api.spotify.com/v1/me/playlists" );

                using( Stream responseStreamAllPlaylists = await message.Content.ReadAsStreamAsync() )
                using( StreamReader readerAllPlaylists = new StreamReader( responseStreamAllPlaylists ) )
                {
                    string allplaylist;
                    
                    string allPlaylistsString = readerAllPlaylists.ReadToEnd();

                    JObject allPlaylistsJson = JObject.Parse( allPlaylistsString );
                    JArray allPlaylistsArray = (JArray) allPlaylistsJson["items"];
                    List<Playlist> listOfPlaylists = new List<Playlist>();

                    for( int i = 0; i < allPlaylistsArray.Count; i++ )
                    {
                        var playlist = allPlaylistsArray[i];

                        string requestTracksInPlaylist = (string) playlist["tracks"]["href"];

                        string idOwner = (string) playlist["owner"]["id"];
                        string name = (string) playlist["name"];
                        string idPlaylist = (string) playlist["id"];
                        string coverPlaylist = (string) playlist["images"][0]["url"];
                        
                        Playlist p = new Playlist( idOwner, idPlaylist, await GetAllTracksInPlaylists( requestTracksInPlaylist, accessToken, idOwner, idPlaylist, coverPlaylist ), name, coverPlaylist );
                        await _playlistService.InsertPlaylist( p );
                        listOfPlaylists.Add( p );                        
                    }
                    allplaylist = JsonConvert.SerializeObject( listOfPlaylists );
                    JToken playlistsJson = JToken.Parse( allplaylist );
                    return playlistsJson;
                }
            }
        }
    }
}