using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OmegaWebApp.Services;
using Microsoft.WindowsAzure.Storage.Table;
using Omega.DAL;
using System.Security.Claims;
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
        //readonly TrackService _trackService;

        public PlaylistController( UserService userService, PlaylistService playlistService, TrackService trackService )
        {
            _userService = userService;
            _playlistService = playlistService;
            //_trackService = trackService;
        }

        // GET: /<controller>/
        [HttpGet( "Playlists" )]
        public async Task<JToken> GetAllPlaylists()
        {
            string guid = User.FindFirst( "www.omega.com:guid" ).Value;
            string spotifyId = await _userService.GetSpotifyId( guid );
            string deezerId = await _userService.GetDeezerId( guid );

            List<Playlist> playlists = await _playlistService.GetAllPlaylistsFromUser( spotifyId, deezerId );
            string allPlaylist = JsonConvert.SerializeObject( playlists );
            JToken playlistsJson = JToken.Parse( allPlaylist );
            return playlistsJson;
        }
    }
}
