using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        // static list of cities that used as a data store
        public List<CityDto> Cities { get; set; }

        //add current data store as a singleton so it can be used throughout the app
        //instead of creating an instance here we can do it in the middleware
        //public static CitiesDataStore Current { get; } = new CitiesDataStore();

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
                    Description = "The Big Apple",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Statue of Liberty",
                            Description = "A colossal neoclassical sculpture on Liberty Island",
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Central Park",
                            Description = "An urban park in New York City",
                        },
                    },
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Los Angeles",
                    Description = "The City of Angels",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Hollywood Sign",
                            Description =
                                "An iconic landmark and cultural symbol located in Los Angeles",
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Griffith Observatory",
                            Description =
                                "A popular observatory offering stunning views of Los Angeles",
                        },
                    },
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Chicago",
                    Description = "The Windy City",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Willis Tower",
                            Description =
                                "A 110-story skyscraper in Chicago, formerly known as Sears Tower",
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Millennium Park",
                            Description =
                                "A public park located in the Loop community area of Chicago",
                        },
                    },
                },
            };
        }
    }
}
