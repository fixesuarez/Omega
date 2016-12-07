using Omega.DAL;
using System.Threading.Tasks;

namespace OmegaWebApp.Services
{
    public class CleanTrackService
    {
        CleanTrackGateway _cleanTrackGateway;
        public CleanTrackService(CleanTrackGateway cleanTrackGateway)
        {
            _cleanTrackGateway = cleanTrackGateway;
        }
        public async Task<CleanTrack> GetSongCleanTrack(string trackId)
        {
            return await _cleanTrackGateway.GetSongCleanTrack(trackId);
        }
    }
}
