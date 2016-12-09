using Microsoft.AspNetCore.Mvc;
using OmegaWebApp.Services;
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
            await _ambianceService.InsertAmbiance(ambiance);
        }

        [HttpGet("DeleteAmbiance")]
        public async Task DeleteAmbiance(string ambiance)
        {
            await _ambianceService.DeleteAmbiance(ambiance);
        }
    }
}
