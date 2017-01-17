using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using OmegaWebApp.Services;
using System.Collections.Generic;
using Omega.DAL;
using Newtonsoft.Json.Linq;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using OmegaWebApp.Authentication;
using System.Linq;

namespace OmegaWebApp.Controllers
{
    [Route("api/[controller]")]
    [Authorize(ActiveAuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme)]
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

            public List<Playlist> AllPlaylists { get; set; }
        }

        [HttpPost("MixPlaylist")]
        public async Task<List<Track>> MixPlaylist( [FromBody]Playlists playlists)
        {
            string email = "";
            if (playlists.AmbianceName == "Lounge" || playlists.AmbianceName == "Energy" || playlists.AmbianceName == "Mad" || playlists.AmbianceName == "Dance")
            {
                email = "allUser";
            }
            else
            {
                email = User.FindFirst("www.omega.com:guid").Value;
            }
            
            Ambiance ambiance = await _ambianceService.RetrieveAmbiance(email, playlists.AmbianceName);
            MetaDonnees metadonnes = new MetaDonnees();
            metadonnes.accousticness = ambiance.Accousticness;
            metadonnes.danceability = ambiance.Danceability;
            metadonnes.instrumentalness = ambiance.Instrumentalness;
            metadonnes.liveness = ambiance.Liveness;
            metadonnes.loudness = ambiance.Loudness;
            metadonnes.mode = ambiance.Mode;
            metadonnes.popularity = ambiance.Popularity;
            metadonnes.speechiness = ambiance.Speechiness;
            metadonnes.valence = ambiance.Valence;
            metadonnes.energy = ambiance.Energy;

            if (playlists.AmbianceName == "Lounge" || playlists.AmbianceName == "Energy" || playlists.AmbianceName == "Mad" || playlists.AmbianceName == "Dance")
            {
                return await PlaylistAnalyser(playlists.AllPlaylists, metadonnes);
            }
            return await PlaylistAnalyser(playlists.AllPlaylists, metadonnes, 10);
        }

        public async Task<List<Track>> PlaylistAnalyser(List<Playlist> playlists, MetaDonnees askedDonnees, double ratio)
        {
            List<string> FilteredList = new List<string>();
            List<Track> filteredArray = new List<Track>();
            List<CleanTrack> cleanTracks = new List<CleanTrack>();
            string trackIdSource;

            foreach (var playlistArray in playlists)
            {
                foreach (var track in playlistArray.Tracks)
                {
                    trackIdSource = track.RowKey.Substring(0,2) + track.TrackId;
                    CleanTrack analysedSong = await _cleanTrackService.GetSongCleanTrack(trackIdSource);
                    if (Compare(askedDonnees.accousticness, analysedSong.Acousticness, ratio)
                    && Compare(askedDonnees.danceability, analysedSong.Danceability, ratio)
                    && Compare(askedDonnees.energy, analysedSong.Energy, ratio)
                    && Compare(askedDonnees.instrumentalness, analysedSong.Instrumentalness, ratio)
                    && Compare(askedDonnees.liveness, analysedSong.Liveness, ratio)
                    && Compare(askedDonnees.loudness, analysedSong.Loudness, ratio)
                    && Compare(askedDonnees.mode, analysedSong.Mode, ratio)
                    && Compare(askedDonnees.speechiness, analysedSong.Speechiness, ratio)
                    && Compare(askedDonnees.tempo, analysedSong.Tempo, ratio)
                    && Compare(askedDonnees.valence, analysedSong.Valence, ratio))
                    {
                        if (!FilteredList.Contains(analysedSong.Id))
                        {
                            if (string.IsNullOrEmpty(analysedSong.DeezerId))
                            {
                                analysedSong.DeezerId = null;
                            }
                            track.DeezerId = analysedSong.DeezerId;
                            track.Popularity = analysedSong.Popularity;
                            if (analysedSong.Popularity != null)
                            {
                                FilteredList.Add(analysedSong.Id);
                                filteredArray.Add(track);
                            }
                        }
                    }
                }
            }

            filteredArray = filteredArray.OrderByDescending(o => o.Popularity).ToList();
            return filteredArray.Where(t=>t.DeezerId!=null ).ToList();
        }

        public async Task<List<Track>> PlaylistAnalyser(List<Playlist> playlists, MetaDonnees askedDonnees)
        {
            List<string> FilteredList = new List<string>();
            List<Track> filteredArray = new List<Track>();
            List<CleanTrack> cleanTracks = new List<CleanTrack>();
            string trackIdSource;

            foreach (var playlistArray in playlists)
            {
                foreach (var track in playlistArray.Tracks)
                {
                    trackIdSource = track.RowKey.Substring(0, 2) + track.TrackId;
                    CleanTrack analysedSong = await _cleanTrackService.GetSongCleanTrack(trackIdSource);
                    if (Compare(askedDonnees.accousticness, analysedSong.Acousticness)
                    && Compare(askedDonnees.danceability, analysedSong.Danceability)
                    && Compare(askedDonnees.energy, analysedSong.Energy)
                    && Compare(askedDonnees.instrumentalness, analysedSong.Instrumentalness)
                    && Compare(askedDonnees.liveness, analysedSong.Liveness)
                    && Compare(askedDonnees.loudness, analysedSong.Loudness)
                    && Compare(askedDonnees.mode, analysedSong.Mode)
                    && Compare(askedDonnees.speechiness, analysedSong.Speechiness)
                    && Compare(askedDonnees.tempo, analysedSong.Tempo)
                    && Compare(askedDonnees.valence, analysedSong.Valence))
                    {
                        if (!FilteredList.Contains(analysedSong.Id))
                        {
                            if (string.IsNullOrEmpty(analysedSong.DeezerId))
                            {
                                analysedSong.DeezerId = null;
                            }
                            track.DeezerId = analysedSong.DeezerId;
                            track.Popularity = analysedSong.Popularity;
                            if (analysedSong.Popularity != null)
                            {
                                FilteredList.Add(analysedSong.Id);
                                filteredArray.Add(track);
                            }
                        }
                    }
                }
            }

            filteredArray = filteredArray.OrderByDescending(o => o.Popularity).ToList();
            return filteredArray.Where(t => t.DeezerId != null).ToList();
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

        public static bool Compare(string asked, string analysed)
        {
            if (asked != null) asked = asked.Replace(".", ",");
            if (analysed != null) analysed = analysed.Replace(".", ",");

            if (string.IsNullOrEmpty(asked) || string.IsNullOrEmpty(analysed))
            {
                return true;
            } else if(Double.Parse(asked) >= 0.5)
            {
                return Double.Parse(analysed) > Double.Parse(asked);
            }else if(Double.Parse(asked) < 0.5)
            {
                return (Double.Parse(analysed) < Double.Parse(asked));
            } else
            {
                return true;
            }
        }
    }
}
