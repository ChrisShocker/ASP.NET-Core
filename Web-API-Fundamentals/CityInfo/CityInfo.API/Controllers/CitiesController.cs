using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;

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
        private readonly IMapper _mapper;

        // inject the contract for the repository and not the implementation
        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository =
                cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public ICityInfoRepository CityInfoRepository { get; }

        // use routing attribute to specify the route for this controller
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities([FromQuery] string? name)
        {
            //var citiesToReturn = _citiesDataStore.Cities;

            //if (citiesToReturn.Count == 0)
            //{
            //    return NotFound();
            //}

            //return Ok(citiesToReturn);

            var cityEntities = await _cityInfoRepository.GetCitiesAsync(name);

            // map the entities to DTOs for the response
            //var results = new List<CityWithoutPointsOfInterestDto>();

            //manually map the entities to DTOs
            //foreach (var city in cityEntities)
            //{
            //    //manual mapping like this is tedious and error-prone
            //    results.Add(
            //        new CityWithoutPointsOfInterestDto
            //        {
            //            Id = city.Id,
            //            Name = city.Name,
            //            Description = city.Description,
            //        }
            //    );
            //}

            //return Ok(results);

            // instead of manually mapping the entities to DTOs, we can use AutoMapper to do it for us
            var results = _mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities);
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);

            if (city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
            var cityResult = _mapper.Map<CityDto>(city);
                return Ok(cityResult);
            }

            //now we need to map the entity to a DTO for the response
            var result = _mapper.Map<CityWithoutPointsOfInterestDto>(city);

            return Ok(result);
        }
    }
}
