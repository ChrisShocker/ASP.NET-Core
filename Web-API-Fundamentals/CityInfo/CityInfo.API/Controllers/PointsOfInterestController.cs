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
        private readonly LocalMailService _mailService;

        // constructor to inject the logger
        public PointsOfInterestController(
            ILogger<PointsOfInterestController> logger,
            LocalMailService mailService
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            //another way to get the logger injected but unpreferred
            /*
            HttpContext.RequestServices.GetService<ILogger<PointsOfInterestController>>()
                ?.LogInformation("PointsOfInterestController created");
            */

            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            try
            {
                var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
                if (city == null)
                {
                    // different levels of the logger can be used to adjust what's logged ie _logger.critical
                    _logger.LogInformation(
                        $"City with id {cityId} wasn't found when accessing points of interest."
                    );
                    return NotFound();
                }

                return Ok(city.PointsOfInterest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    $"Exception while getting points of interest for city with id {cityId}.",
                    ex
                );
                // do not expose exception details to the client, exposing implementation details is bad
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        // add a name so this endpoint can be referenced in other endpoints
        [HttpGet("{pointsOfInterestId}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(
            int cityId,
            int pointsOfInterestId
        )
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p =>
                p.Id == pointsOfInterestId
            );
            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterest);
        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(
            int cityId,
            PointOfInterestForCreationDto pointOfInterest
        )
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var maxPointOfInterestId = CitiesDataStore
                .Current.Cities.SelectMany(c => c.PointsOfInterest)
                .Max(p => p.Id);

            var finalPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description,
            };

            //return uri of where the new point of interest can be found
            return CreatedAtRoute(
                "GetPointOfInterest",
                new { cityId = cityId, pointsOfInterestId = finalPointOfInterest.Id },
                finalPointOfInterest
            );
        }

        [HttpPut("{pointOfInterestId}")]
        public ActionResult<PointOfInterestUpdateDto> UpdatePointOfInterest(
            int cityId,
            int pointOfInterestId,
            PointOfInterestUpdateDto pointOfInterest
        )
        {
            // find existing city
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            // find existing point of interest
            var pointOfInterestToUpdate = city.PointsOfInterest.FirstOrDefault(p =>
                p.Id == pointOfInterestId
            );

            if (pointOfInterestToUpdate == null)
            {
                return NotFound();
            }

            pointOfInterestToUpdate.Name = pointOfInterest.Name;
            pointOfInterestToUpdate.Description = pointOfInterest.Description;

            return Ok(
                new PointOfInterestDto()
                {
                    Id = pointOfInterestToUpdate.Id,
                    Name = pointOfInterestToUpdate.Name,
                    Description = pointOfInterestToUpdate.Description,
                }
            );
        }

        [HttpPatch("{pointOfInterestId}")]
        public ActionResult<PointOfInterestUpdateDto> PartiallyUpdatePointOfInterest(
            int cityId,
            int pointOfInterestId,
            JsonPatchDocument<PointOfInterestUpdateDto> patchDocument
        )
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromDataStore = city.PointsOfInterest.FirstOrDefault(p =>
                p.Id == pointOfInterestId
            );

            if (pointOfInterestFromDataStore == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = new PointOfInterestUpdateDto()
            {
                Name = pointOfInterestFromDataStore.Name,
                Description = pointOfInterestFromDataStore.Description,
            };

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            pointOfInterestFromDataStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromDataStore.Description = pointOfInterestToPatch.Description;

            return Ok(
                new PointOfInterestDto()
                {
                    Id = pointOfInterestFromDataStore.Id,
                    Name = pointOfInterestFromDataStore.Name,
                    Description = pointOfInterestFromDataStore.Description,
                }
            );
        }

        [HttpDelete("{pointOfInterestId}")]
        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestToDelete = city.PointsOfInterest.FirstOrDefault(p =>
                p.Id == pointOfInterestId
            );

            if (pointOfInterestToDelete == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(pointOfInterestToDelete);

            // use mail service to send a notification about the deletion
            _mailService.Send(
                "Point of interest deleted",
                $"Point of interest {pointOfInterestToDelete.Name} with id {pointOfInterestToDelete.Id} was deleted."
            );

            return NoContent();
        }
    }
}
