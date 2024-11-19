using Microsoft.AspNetCore.Mvc;
using Prova.InputModel;
using Prova.Services;

namespace Prova.Controllers
{
    [ApiController]
    [Route("api/placemarks")]
    public class PlacemarkController : ControllerBase
    {
        private readonly PlacemarkService _placemarkService;

        public PlacemarkController(PlacemarkService placemarkService)
        {
            _placemarkService = placemarkService;
        }

        [HttpPost("export")]
        public IActionResult ExporttFilteredKml([FromBody] FilterRequestInputModel filterRequestInputModel)
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult GetFilteredPlacemarks([FromBody] FilterRequestInputModel filterRequestInputModel)
        {
            return Ok();
        }

        [HttpGet("filters")]
        public IActionResult GetAvailableFilters()
        {
            return Ok();
        }

    }
}
