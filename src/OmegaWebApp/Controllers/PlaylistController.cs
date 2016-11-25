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

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OmegaWebApp.Controllers
{
    [Route( "api/[controller]" )]
    public class PlaylistController : Controller
    {
        readonly UserService _userService;
        readonly PlaylistService _playlistService;
        readonly TrackService _trackService;

        public PlaylistController( UserService userService, PlaylistService playlistService, TrackService trackService )
        {
            _userService = userService;
            _playlistService = playlistService;
            _trackService = trackService;
        }

        // GET: /<controller>/
        public async Task<JToken> GetAllPlaylists()
        {
            string email = User.FindFirst( ClaimTypes.Email ).Value;
            string spotifyId = await _userService.GetSpotifyId( email );
            string deezerId = await _userService.GetDeezerId( email );

            TableQuery<Playlist> query = new TableQuery<Playlist>().Where( TableQuery.GenerateFilterCondition( "PartitionKey", QueryComparisons.Equal, "Smith" ) );

            //// Print the fields for each customer.
            //foreach( CustomerEntity entity in table.ExecuteQuery( query ) )
            //{
            //    Console.WriteLine( "{0}, {1}\t{2}\t{3}", entity.PartitionKey, entity.RowKey,
            //        entity.Email, entity.PhoneNumber );
            //}
            //return null;
        }
    }
}
