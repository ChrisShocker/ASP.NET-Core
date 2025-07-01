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
        public JsonResult GetCities()
        {
            // this is a simple example of returning JSON data
            return new JsonResult(new[]
            {
                new { Id = 1, Name = "New York" },
                new { Id = 2, Name = "Los Angeles" },
                new { Id = 3, Name = "Chicago" }
            });
        }
    }
}
