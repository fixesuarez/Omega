﻿using System.Threading.Tasks;

namespace Omega.Crawler
{
    public class Analyser
    {
        public async Task AnalyseNewSong(Controller c, string trackId, string source)
        {
            if(source == "s")
            {
                MetaDonnees meta = await c.GetCredentialAuth().TrackMetadonnee(trackId);
                await Task.Delay(500);
                //Thread.Sleep(1000);
                Track track = await c.GetGetATrack().GetTrackSpotify(trackId);
                string deezerId = await c.SpotifyToDeezer().GetDeezerId(track.Title, track.Artist);
                c.GetRequests().AddSongCleanTrack(meta,track.Artist, deezerId, trackId, track.Title, source, track.AlbumName, track.Popularity);
            }
            else if (source == "d")
            {
                Track dm = await c.GetDeezerConnect().Connect(trackId);
                string spotifyId = await c.GetSpotifycation().Search(dm.Title, dm.Artist, dm.AlbumName);
                if(spotifyId != "")
                {
                    MetaDonnees meta = await c.GetCredentialAuth().TrackMetadonnee(spotifyId);
                    //Thread.Sleep(1000);
                    await Task.Delay(500);
                    Track track = await c.GetGetATrack().GetTrackSpotify(spotifyId);
                    c.GetRequests().AddSongCleanTrack(meta, track.Artist, trackId, trackId, track.Title, source, track.AlbumName, track.Popularity);
                }             
            }
        }

        public async Task AnalyseSong(Controller c, string trackId, string source)
        {
            if (source == "s")
            {
                MetaDonnees meta = await c.GetCredentialAuth().TrackMetadonnee(trackId);
                //Thread.Sleep(1000);
                await Task.Delay(500);
                Track track = await c.GetGetATrack().GetTrackSpotify(trackId);
                c.GetRequests().UpdateCleanTrack(meta, trackId, track.Title, source, track.AlbumName, track.Popularity);
            }
            else if (source == "d")
            {
                Track dm = await c.GetDeezerConnect().Connect(trackId);
                string spotifyId = await c.GetSpotifycation().Search(dm.Title, dm.Artist, dm.AlbumName);
                //Thread.Sleep(1000);
                await Task.Delay(500);
                if (spotifyId != "")
                {
                    MetaDonnees meta = await c.GetCredentialAuth().TrackMetadonnee(spotifyId);
                    Track track = await c.GetGetATrack().GetTrackSpotify(spotifyId);
                    c.GetRequests().UpdateCleanTrack(meta, trackId, track.Title, source, track.AlbumName, track.Popularity);
                }
            }
        }
    }
}
