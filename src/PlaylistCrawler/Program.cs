using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using Omega.DAL;

namespace PlaylistCrawler
{
    public class Program
    {
        static IConfigurationRoot _configuration;
        static TrackGateway _trackGateway;
        static PlaylistGateway _playlistGateway;
        static SpotifyApiService _spotifyApiService;
        static DeezerApiService _deezerApiService;
        static UserGateway _userGateway;
        public static void Main(string[] args)
        {
            _configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true)
               .AddEnvironmentVariables()
               .Build();
            _playlistGateway = new PlaylistGateway(_configuration["data:azure:ConnectionString"]);
            _trackGateway = new TrackGateway(_configuration["data:azure:ConnectionString"]);
            _userGateway = new UserGateway(_configuration["data:azure:ConnectionString"]);
            _spotifyApiService = new SpotifyApiService(_trackGateway, _playlistGateway, _userGateway);
            _deezerApiService = new DeezerApiService(_trackGateway, _playlistGateway, _userGateway);
            //_spotifyApiService.GetSpotifyPlaylist(guid).Wait();
            _deezerApiService.GetAllDeezerPlaylists("b555241b-24ba-41dc-b015-03bf97fc8d7e").Wait();
        }
    }
}
