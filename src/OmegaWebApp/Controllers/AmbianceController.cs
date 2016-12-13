using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("InsertAmbiance")]
        public async Task InsertAmbiance(string ambiance)
        {
            string email = User.FindFirst(ClaimTypes.Email).Value;
            await _ambianceService.InsertAmbiance(email, ambiance);
        }

        [HttpGet("DeleteAmbiance")]
        public async Task DeleteAmbiance(string ambiance)
        {
            string email = User.FindFirst(ClaimTypes.Email).Value;
            await _ambianceService.DeleteAmbiance(email, ambiance);
        }
    }
}
