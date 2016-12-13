using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Omega.DAL;
using OmegaWebApp.Services;
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

        [HttpGet("DeleteAmbiance")]
        public async Task DeleteAmbiance(string ambiance)
        {
            string email = User.FindFirst(ClaimTypes.Email).Value;
            await _ambianceService.DeleteAmbiance(email, ambiance);
        }
    }
}
