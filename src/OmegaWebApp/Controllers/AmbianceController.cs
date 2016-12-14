using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Omega.DAL;
using OmegaWebApp.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OmegaWebApp.Controllers
{
    [Route("api/[controller]")]
    public class AmbianceController : Controller
    {
        readonly AmbianceService _ambianceService;
        public AmbianceController(AmbianceService ambianceService)
        {
            _ambianceService = ambianceService;
        }

        public class Mood
        {
            public MetaDonnees Metadonnees { get; set; }
            public string Name { get; set; }

            public string Cover { get; set; }

            public static string StringifyMood( Mood m )
            {
                return JsonConvert.SerializeObject(m);
            }
        }

        [HttpPost("InsertAmbiance")]
        public async Task InsertAmbiance([FromBody]Mood ambiance)
        {
            string email = User.FindFirst(ClaimTypes.Email).Value;
            await _ambianceService.InsertAmbiance(email, Mood.StringifyMood(ambiance));
        }

        [HttpPost("DeleteAmbiance")]
        public async Task DeleteAmbiance(Mood ambiance)
        {
            string email = User.FindFirst(ClaimTypes.Email).Value;
            await _ambianceService.DeleteAmbiance(email, Mood.StringifyMood(ambiance));
        }

        [HttpGet("RetrieveAllUserAmbiance")]
        public async Task<List<NewAmbiance>> RetrieveAllUserAmbiance()
        {
            string email = User.FindFirst(ClaimTypes.Email).Value;
            List<Ambiance> ambiances = await _ambianceService.RetrieveAllUSerAmbiance(email);
            List<NewAmbiance> newAmbiances = new List<NewAmbiance>();
            
            foreach (Ambiance ambiance in ambiances)
            {
                NewMetadonnees metadonnees = new NewMetadonnees();
                NewAmbiance newAmbiance = new NewAmbiance(ambiance.PartitionKey, ambiance.RowKey);

                newAmbiance.Cover = ambiance.Cover;
                metadonnees.acousticness = ambiance.Acousticness;
                metadonnees.danceability = ambiance.Danceability;
                metadonnees.energy = ambiance.Energy;
                metadonnees.instrumentalness = ambiance.Instrumentalness;
                metadonnees.liveness = ambiance.Liveness;
                metadonnees.loudness = ambiance.Loudness;
                metadonnees.mode = ambiance.Mode;
                metadonnees.speechiness = ambiance.Speechiness;
                metadonnees.popularity = ambiance.Popularity;
                newAmbiance.Metadonnees = metadonnees;
                newAmbiances.Add(newAmbiance);
            }
            return newAmbiances;
        }
    }
}
