using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Omega.DAL;
using OmegaWebApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OmegaWebApp.Controllers
{
    [Route( "api/[controller]" )]
    public class DeezerController : Controller
    {
        readonly PlaylistService _playlistService;
        readonly TrackService _trackService;
        readonly UserService _userService;

        public DeezerController( PlaylistService playlistService, TrackService trackService, UserService userService )
        {
            _playlistService = playlistService;
            _trackService = trackService;
            _userService = userService;
        }

        /// /// <summary>
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
                List<Track> tracksInPlaylist = new List<Track>();
                Uri tracksInPlaylistUri = new Uri( String.Format(
                    "{0}?output=json&access_token={1}",
                    urlRequest,
                    accessToken ) );
                HttpResponseMessage message = await client.GetAsync( tracksInPlaylistUri );

                using( Stream responseStream = await message.Content.ReadAsStreamAsync() )
                using( StreamReader reader = new StreamReader( responseStream ) )
                {
                    string allTracksInPlaylistString = reader.ReadToEnd();
                    JObject allTracksInPlaylistJson = JObject.Parse( allTracksInPlaylistString );
                    JArray allTracksInPlaylistArray = (JArray) allTracksInPlaylistJson["data"];

                    for( int i = 0; i < allTracksInPlaylistArray.Count; i++ )
                    {
                        var track = allTracksInPlaylistArray[i];

                        string trackTitle = (string) track["title"];
                        string trackId = (string) track["id"];
                        string albumName = (string) track["album"]["title"];
                        string trackRank = (string) track["rank"];
                        string duration = (string) track["duration"];
                        string coverAlbum = (string) track["album"]["cover"];

                        if( await _trackService.GetTrack("d", playlistId, trackId) == null )
                            await _trackService.InsertTrack( "d",  playlistId, trackId, trackTitle, albumName, trackRank, duration, coverAlbum );
                        tracksInPlaylist.Add( new Track( "d", playlistId, trackId, trackTitle, albumName, trackRank, duration, coverAlbum ) );
                    }
                }
                return tracksInPlaylist;
            }
        }

        [HttpGet( "Playlists" )]
        public async Task<JToken> GetAllDeezerPlaylists()
        {
            using( HttpClient client = new HttpClient() )
            {
                string email = User.FindFirst( ClaimTypes.Email ).Value;
                string accessToken = await _userService.GetDeezerAccessToken( email );

                Uri allPlaylistsUri = new Uri( string.Format(
                "http://api.deezer.com/user/me/playlists?output=json&access_token={0}",
                accessToken ) );

                HttpResponseMessage message = await client.GetAsync( allPlaylistsUri );

                using( Stream responseStream = await message.Content.ReadAsStreamAsync() )
                using( StreamReader reader = new StreamReader( responseStream ) )
                {
                    JObject allPlaylistsJson;
                    string allplaylist;

                    string allPlaylistsString = reader.ReadToEnd();
                    allPlaylistsJson = JObject.Parse( allPlaylistsString );
                    JArray allPlaylistsArray = (JArray) allPlaylistsJson["data"];

                    List<Playlist> listOfPlaylists = new List<Playlist>();

                    for( int i = 0; i < allPlaylistsArray.Count; i++ )
                    {
                        var playlist = allPlaylistsArray[i];

                        string requestTracksInPlaylist = (string) playlist["tracklist"];
                        requestTracksInPlaylist = requestTracksInPlaylist.Replace( "https", "http" );

                        string idOwner = (string) playlist["creator"]["id"];
                        string name = (string) playlist["title"];
                        string idPlaylist = (string) playlist["id"];
                        string coverPlaylist = (string) playlist["picture"];

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
