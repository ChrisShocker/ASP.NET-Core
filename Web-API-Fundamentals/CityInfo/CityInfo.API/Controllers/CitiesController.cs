using CityInfo.API.Models;
using CityInfo.API.Services;
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
        private readonly ICityInfoRepository _cityInfoRepository;

        // inject the contract for the repository and not the implementation
        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository =
                cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        }

        public ICityInfoRepository CityInfoRepository { get; }

        // use routing attribute to specify the route for this controller
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities()
        {
            //var citiesToReturn = _citiesDataStore.Cities;

            //if (citiesToReturn.Count == 0)
            //{
            //    return NotFound();
            //}

            //return Ok(citiesToReturn);

            var cityEntities = await _cityInfoRepository.GetCitiesAsync();

            // map the entities to DTOs for the response
            var results = new List<CityWithoutPointsOfInterestDto>();

            //manually map the entities to DTOs
            foreach (var city in cityEntities)
            {
                //manual mapping like this is tedious and error-prone
                results.Add(
                    new CityWithoutPointsOfInterestDto
                    {
                        Id = city.Id,
                        Name = city.Name,
                        Description = city.Description,
                    }
                );
            }

            return Ok(results);
        }

        //[HttpGet("{id}")]
        //public ActionResult<CityDto> GetCity(int id)
        //{
        //    var cityToReturn = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == id);

        //    if (cityToReturn == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(cityToReturn);
        //}
    }
}
