using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        // static list of cities that used as a data store
        public List<CityDto> Cities { get; set; }

        //add current data store as a singleton so it can be used throughout the app
        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        // constructor to initialize the data store with some sample cities
        public CitiesDataStore()
        {
            // initialize the Cities list with some sample data
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "New York",
                    Description = "The Big Apple"
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Los Angeles",
                    Description = "The City of Angels"
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Chicago",
                    Description = "The Windy City"
                }


            };
        }
    }
}
