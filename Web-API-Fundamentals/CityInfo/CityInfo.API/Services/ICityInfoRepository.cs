using CityInfo.API.Entities;

namespace CityInfo.API.Services
{
    //Repository contract defines the methods that the repository will implement
    public interface ICityInfoRepository
    {
        // Can be type IQueryable to allow further querying, this leaks persistence details
        // arguments can be made for each type and why they're better

        // sync version
        // IEnumerable<City> GetCities();

        // async version
        Task<IEnumerable<City>> GetCitiesAsync();

        Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);

        Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);

        Task<IEnumerable<PointOfInterest?>> GetPointsOfInterestForCityAsync(int cityId);
    }
}
