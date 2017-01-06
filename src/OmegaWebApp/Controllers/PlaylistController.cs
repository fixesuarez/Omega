using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OmegaWebApp.Services;
using Omega.DAL;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using OmegaWebApp.Authentication;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OmegaWebApp.Controllers
{
    [Route( "api/[controller]" )]
    [Authorize(ActiveAuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme)]
    public class PlaylistController : Controller
    {
        readonly UserService _userService;
        readonly PlaylistService _playlistService;
        readonly EventGroupService _eventGroupService;

        public PlaylistController( UserService userService, PlaylistService playlistService, EventGroupService eventGroupService )
        {
            _userService = userService;
            _playlistService = playlistService;
            _eventGroupService = eventGroupService;
        }

        // GET: /<controller>/
        [HttpGet( "Playlists" )]
        public async Task<JToken> GetAllPlaylistsFromUser()
        {
            string guid = User.FindFirst( "www.omega.com:guid" ).Value;
            string spotifyId = await _userService.GetSpotifyId( guid );
            string deezerId = await _userService.GetDeezerId( guid );

            List<Playlist> playlists = await _playlistService.GetAllPlaylistsFromUser( spotifyId, deezerId );
            string allPlaylist = JsonConvert.SerializeObject( playlists );
            JToken playlistsJson = JToken.Parse( allPlaylist );
            return playlistsJson;
        }
        public async Task<List<Playlist>> GetAllPlaylistsFromUser( string guid )
        {
            string spotifyId = await _userService.GetSpotifyId( guid );
            string deezerId = await _userService.GetDeezerId( guid );

            List<Playlist> playlists = await _playlistService.GetAllPlaylistsFromUser( spotifyId, deezerId );
            return playlists;
        }

        // GET: /<controller>/
        [HttpGet( "EventOrGroup/{idEventGroup}" )]
        public async Task<JToken> GetAllPlaylistsFromGroupOrEvent( string idEventGroup)
        {
            List<Playlist> allPlaylists = new List<Playlist>();
            string guid = User.FindFirst( "www.omega.com:guid" ).Value;
            List<EventGroup> membersFromEventGroup = await _eventGroupService.GetAllMembersFromEventGroup( idEventGroup );
            foreach( var member in membersFromEventGroup )
            {
                allPlaylists.AddRange( await GetAllPlaylistsFromUser( member.RowKey ) );
            }
            string allPlaylistsFromUser = JsonConvert.SerializeObject( allPlaylists );
            JToken playlistsJson = JToken.Parse( allPlaylistsFromUser );
            return playlistsJson;
        }
    }
}
