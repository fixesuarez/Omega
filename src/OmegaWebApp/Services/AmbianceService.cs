using Newtonsoft.Json.Linq;
using Omega.DAL;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OmegaWebApp.Services
{
    public class AmbianceService
    {
        AmbianceGateway _ambianceGateway;
        public AmbianceService(AmbianceGateway ambianceGateway)
        {
            _ambianceGateway = ambianceGateway;
        }

        public async Task InsertAmbiance(string user, string ambiance)
        {
            JObject rss = JObject.Parse(ambiance);
            string name = (string)rss["Name"];
            string cover = (string)rss["Cover"];
            MetaDonnees metadonnees = new MetaDonnees();
            metadonnees.accousticness = (string)rss["Metadonnees"]["accousticness"];
            metadonnees.danceability = (string)rss["Metadonnees"]["danceability"];
            metadonnees.instrumentalness = (string)rss["Metadonnees"]["instrumentalness"];
            metadonnees.liveness = (string)rss["Metadonnees"]["liveness"];
            metadonnees.popularity = (string)rss["Metadonnees"]["popularity"];
            metadonnees.speechiness = (string)rss["Metadonnees"]["speechiness"];
            metadonnees.energy = (string)rss["Metadonnees"]["energy"];
            await _ambianceGateway.InsertAmbiance(user, name,cover, metadonnees);
        }

        public async Task DeleteAmbiance(string user, string ambianceName)
        {
            await _ambianceGateway.DeleteAmbiance(user, ambianceName);
        }

        public async Task<Ambiance> RetrieveAmbiance(string user, string ambiance)
        {
            //JObject rss = JObject.Parse(ambiance);
            return await _ambianceGateway.RetrieveAmbiance(user, ambiance);
        }

        public async Task<List<Ambiance>> RetrieveAllUSerAmbiance(string userName)
        {
            return await _ambianceGateway.RetrieveAllUserAmbiance(userName);
        }
    }
}
