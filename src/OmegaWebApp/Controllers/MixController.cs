using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using OmegaWebApp.Services;
using System.Collections.Generic;
using Omega.DAL;
using Newtonsoft.Json.Linq;
using System;
using System.Security.Claims;

namespace OmegaWebApp.Controllers
{
    [Route("api/[controller]")]
    public class MixController : Controller
    {
        readonly AmbianceService _ambianceService;
        readonly CleanTrackService _cleanTrackService;
        public MixController(AmbianceService ambianceService, CleanTrackService cleanTrackService)
        {
            _ambianceService = ambianceService;
            _cleanTrackService = cleanTrackService;
        }

        public class Playlists
        {
            public string AmbianceName { get; set; }

            public string AllPlaylists { get; set; }
        }

        [HttpGet("MixPlaylist")]
        public async Task<JArray> MixPlaylist( [FromBody]Playlists playlists)
        {
            string email = "";
            if (playlists.AmbianceName == "Lounge" || playlists.AmbianceName == "Energy" || playlists.AmbianceName == "Mad" || playlists.AmbianceName == "Dance")
            {
                email = "allUser";
            }
            else
            {
                email = User.FindFirst(ClaimTypes.Email).Value;
            }
            
            Ambiance ambiance = await _ambianceService.RetrieveAmbiance(email, playlists.AmbianceName);
            MetaDonnees metadonnes = new MetaDonnees();
            metadonnes.acousticness = ambiance.Acousticness;
            metadonnes.danceability = ambiance.Danceability;
            metadonnes.instrumentalness = ambiance.Instrumentalness;
            metadonnes.liveness = ambiance.Liveness;
            metadonnes.loudness = ambiance.Loudness;
            metadonnes.mode = ambiance.Mode;
            metadonnes.popularity = ambiance.Popularity;
            metadonnes.speechiness = ambiance.Speechiness;
            metadonnes.tempo = ambiance.Tempo;
            metadonnes.valence = ambiance.Valence;
            return await PlaylistAnalyser(playlists.AllPlaylists, metadonnes);
        }

        public async Task<JArray> PlaylistAnalyser(string playlists, MetaDonnees askedDonnees, double ratio = 10)
        {
            List<string> FilteredList = new List<string>();
            JArray filteredArray = new JArray();
            List<CleanTrack> cleanTracks = new List<CleanTrack>();
            string trackIdSource;
            JArray playlistObj = JArray.Parse(playlists);
     
            foreach (var playlistArray in playlistObj)
            {
                foreach (var track in playlistArray)
                {
                    trackIdSource = track["RowKey"].ToString().Substring(0, 1) + ":" + track["TrackId"].ToString();
                    CleanTrack analysedSong = await _cleanTrackService.GetSongCleanTrack(trackIdSource);
                    if (Compare(askedDonnees.acousticness, analysedSong.Acousticness, ratio)
                    && Compare(askedDonnees.danceability, analysedSong.Danceability, ratio)
                    && Compare(askedDonnees.energy, analysedSong.Energy, ratio)
                    && Compare(askedDonnees.instrumentalness, analysedSong.Instrumentalness, ratio)
                    && Compare(askedDonnees.liveness, analysedSong.Liveness, ratio)
                    && Compare(askedDonnees.loudness, analysedSong.Loudness, ratio)
                    && Compare(askedDonnees.mode, analysedSong.Mode, ratio)
                    && Compare(askedDonnees.speechiness, analysedSong.Speechiness, ratio)
                    && Compare(askedDonnees.tempo, analysedSong.Tempo, ratio)
                    && Compare(askedDonnees.valence, analysedSong.Valence, ratio))
                        if (!FilteredList.Contains(analysedSong.Id))
                        {
                            FilteredList.Add(analysedSong.Id);
                            filteredArray.Add(track);
                        }
                }
            }
            return filteredArray;
        }

        public static bool Compare(string asked, string analysed, double ratio)
        {
            if (!string.IsNullOrEmpty(asked))
                ratio = ratio / 100;
            if (asked != null) asked = asked.Replace(".", ",");
            if (analysed != null) analysed = analysed.Replace(".", ",");
            //if (double.Parse(analysed) < 0) analysed = (double.Parse(analysed) *(-1)).ToString();

            return (string.IsNullOrEmpty(asked) || string.IsNullOrEmpty(analysed) || double.Parse(analysed) < 0 || double.Parse(asked) == 0
                || (Double.Parse(analysed) > Double.Parse(asked) - ratio && Double.Parse(analysed) < Double.Parse(asked) + ratio));
        }
    }
}
