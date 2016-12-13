using Newtonsoft.Json.Linq;
using Omega.DAL;
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
            await _ambianceGateway.InsertAmbiance(user, (string)rss["name"], (string)rss["metadonnees"]);
        }

        public async Task DeleteAmbiance(string user, string ambiance)
        {
            JObject rss = JObject.Parse(ambiance);
            await _ambianceGateway.DeleteAmbiance(user, (string)rss["name"]);
        }

        public async Task<Ambiance> RetrieveAmbiance(string user, string ambiance)
        {
            JObject rss = JObject.Parse(ambiance);
            return await _ambianceGateway.RetrieveAmbiance(user, ambiance);
        }
    }
}
