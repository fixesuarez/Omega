using Omega.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OmegaWebApp.Services
{
    public class PlaylistService
    {
        PlaylistGateway _playlistGateway;

        public PlaylistService( PlaylistGateway playlistGateway )
        {
            _playlistGateway = playlistGateway;
        }

        public async Task InsertPlaylist( Playlist p )
        {
            await _playlistGateway.InsertPlaylist( p );
        }

        public async Task<List<Playlist>> GetAllPlaylistsFromUser( string spotifyId, string deezerId )
        {
            return await _playlistGateway.RetrieveAllPlaylistsFromUser( spotifyId, deezerId );
        }
    }
}
