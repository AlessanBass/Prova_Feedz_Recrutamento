using Microsoft.AspNetCore.Mvc;
using Prova.InputModel;
using Prova.Services;
using Prova.ViewModels;
using AutoMapper;

namespace Prova.Controllers
{
    [ApiController]
    [Route("api/placemarks")]
    public class PlacemarkController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly PlacemarkService _placemarkService;

        public PlacemarkController(PlacemarkService placemarkService, IMapper mapper)
        {
            _placemarkService = placemarkService;
            _mapper = mapper;
        }

        [HttpPost("export")]
        public IActionResult ExporttFilteredKml([FromBody] FilterRequestInputModel filterRequestInputModel)
        {
            return Ok();
        }

        [HttpGet]
        public ActionResult<PlacemarkViewModel> GetFilteredPlacemarks([FromQuery] FilterRequestInputModel filterRequestInputModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var listPlacemarks = _placemarkService.FilterPlacemarks(filterRequestInputModel);

            return Ok(_mapper.Map<IEnumerable<PlacemarkViewModel>>(listPlacemarks));
        }

        [HttpGet("filters")]
        public ActionResult<FiltersViewModel> GetAvailableFilters()
        {
            var filtersDto = _placemarkService.GetAvailableFilters();
            return Ok(_mapper.Map<FiltersViewModel>(filtersDto));
        }

    }
}
