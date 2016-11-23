using Omega.DAL;
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
    }
}
