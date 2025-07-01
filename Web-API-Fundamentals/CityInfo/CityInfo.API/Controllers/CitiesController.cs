using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    //use APIController attribute to add support for automatic model validation, binding source inference, and more
    [ApiController]

    /*
     * specify the base route for this controller
     * [Route<"api/[controller]">] can be used to
     * dynamically set the controller name in the route
    */
    [Route("api/cities")] 
    public class CitiesController : ControllerBase
    {
        // use routing attribute to specify the route for this controller
        [HttpGet]
        public ActionResult<CityDto[]> GetCities()
        {

            var citiesToReturn = CitiesDataStore.Current.Cities;

            if (citiesToReturn.Count == 0)
            {
                return NotFound();
            }

            return Ok(citiesToReturn);
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            var cityToReturn = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);

            if (cityToReturn == null) { return NotFound(); }

            return Ok(cityToReturn);
        }
    }
}
