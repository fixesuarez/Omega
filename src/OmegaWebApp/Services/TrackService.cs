﻿using Omega.DAL;
using System.Threading.Tasks;

namespace OmegaWebApp.Services
{
    public class TrackService
    {
        TrackGateway _trackGateway;

        public TrackService( TrackGateway trackGateway )
        {
            _trackGateway = trackGateway;
        }

        public async Task InsertTrack( string source, string playlistId, string trackId, string title, string albumName, string popularity, string duration, string cover )
        {
            await _trackGateway.InsertTrack( source, playlistId, trackId, title, albumName, popularity, duration, cover );
        }

        public async Task<Track> GetTrack( string source, string idPlaylist, string idTrack )
        {
            return await _trackGateway.RetrieveTrack( source, idPlaylist, idTrack );
        }
    }
}
