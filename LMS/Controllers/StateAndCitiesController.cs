using Data.Repositary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateAndCitiesController : ControllerBase
    {

        private IStateandCities _stateandCities;

        public StateAndCitiesController(IStateandCities stateandCities)
        {
            _stateandCities = stateandCities;
        }

        [HttpGet]
        public async Task<IActionResult> GetState()
        {
            var state = await _stateandCities.GetState();
            return Ok(state);
        }



        [HttpGet]
        [Route("cities/{stateId}")]
        public async Task<IActionResult> GetCitiesByStateId(int stateId)
        {
            var cities = await _stateandCities.GetCitiesByStateId(stateId);

            if (cities == null)
            {
                return NotFound();
            }

            return Ok(cities);
        }



    }
}
