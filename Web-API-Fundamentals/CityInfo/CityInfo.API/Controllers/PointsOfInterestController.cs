using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    //child of another controller, so it can inherit the same route prefix
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        // inject the logger to log messages
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        // constructor to inject the logger
        public PointsOfInterestController(
            ILogger<PointsOfInterestController> logger,
            IMailService mailService,
            ICityInfoRepository cityInfoRepository,
            IMapper mapper

        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            //another way to get the logger injected but unpreferred
            /*
            HttpContext.RequestServices.GetService<ILogger<PointsOfInterestController>>()
                ?.LogInformation("PointsOfInterestController created");
            */

            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));

            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {

            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation(
                    $"City with id {cityId} wasn't found when accessing points of interest."
                );
                return NotFound();
            }

            var pointsOfInterestForCity = await _cityInfoRepository.GetPointsOfInterestForCityAsync(cityId);

            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));

            // out with the old, in with the new, use the repository to get the points of interest for the city
            //try
            //{
            //    var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            //    if (city == null)
            //    {
            //        // different levels of the logger can be used to adjust what's logged ie _logger.critical
            //        _logger.LogInformation(
            //            $"City with id {cityId} wasn't found when accessing points of interest."
            //        );
            //        return NotFound();
            //    }

            //    return Ok(city.PointsOfInterest);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogCritical(
            //        $"Exception while getting points of interest for city with id {cityId}.",
            //        ex
            //    );
            //    // do not expose exception details to the client, exposing implementation details is bad
            //    return StatusCode(500, "A problem happened while handling your request.");
            //}
        }

        // add a name so this endpoint can be referenced in other endpoints
        [HttpGet("{pointsOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(
            int cityId,
            int pointsOfInterestId
        )
        {

            var city = await _cityInfoRepository.GetCityAsync(cityId, false);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterest = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointsOfInterestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(
                _mapper.Map<PointOfInterestDto>(
                    pointOfInterest
                )
            );

            //var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            //var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p =>
            //    p.Id == pointsOfInterestId
            //);
            //if (pointOfInterest == null)
            //{
            //    return NotFound();
            //}

            //return Ok(pointOfInterest);
        }

        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(
            int cityId,
            PointOfInterestForCreationDto pointOfInterest
        )
        {
            var city = await _cityInfoRepository.GetCityAsync(cityId, false);

            if (city == null)
            {
                return NotFound();
            }

            var finalPointOfInterest = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);

            // the repository will assign an id to the new point of interest
            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, finalPointOfInterest);

            // save the changes to the database
            await _cityInfoRepository.SaveChangesAsync();

            // map the new point of interest to a DTO to return to the client
            var createdPointOfInterestToReturn = _mapper.Map<PointOfInterestDto>(finalPointOfInterest);

            //return uri of where the new point of interest can be found
            return CreatedAtRoute(
                "GetPointOfInterest",
                new { cityId = cityId, pointsOfInterestId = createdPointOfInterestToReturn.Id },
                createdPointOfInterestToReturn
            );
        }

        [HttpPut("{pointOfInterestId}")]
        public async Task<ActionResult> UpdatePointOfInterest(
            int cityId,
            int pointOfInterestId,
            PointOfInterestUpdateDto pointOfInterest
        )
        {



            // find existing city
            var cityExists = await _cityInfoRepository.CityExistsAsync(cityId);

            if (!cityExists)
            {
                return NotFound();
            }

            // find existing point of interest
            var pointOfInterestToUpdate = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointOfInterestToUpdate == null)
            {
                return NotFound();
            }

            // map the updated point of interest to the existing point of interest, this will update the existing point of interest with the new values
            _mapper.Map(pointOfInterest, pointOfInterestToUpdate);

            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{pointOfInterestId}")]
        public async Task<ActionResult<PointOfInterestUpdateDto>> PartiallyUpdatePointOfInterest(
            int cityId,
            int pointOfInterestId,
            JsonPatchDocument<PointOfInterestUpdateDto> patchDocument
        )
        {
            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if(pointOfInterestEntity == null) { return NotFound(); }

            var pointOfInterestToPatch = _mapper.Map<PointOfInterestUpdateDto>(pointOfInterestEntity);

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = await _cityInfoRepository.GetCityAsync(cityId, false);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestToEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointOfInterestToEntity == null)
            {
                return NotFound();
            }



            _cityInfoRepository.DeletePointOfInterest(pointOfInterestToEntity);

            await _cityInfoRepository.SaveChangesAsync();

            // use mail service to send a notification about the deletion
            _mailService.Send(
                "Point of interest deleted",
                $"Point of interest {pointOfInterestToEntity.Name} with id {pointOfInterestToEntity.Id} was deleted."
            );

            return NoContent();
        }
    }
}
