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

        Task<IEnumerable<City>> GetCitiesAsync(string? name);

        Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);

        Task<bool> CityExistsAsync(int cityId);

        Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);

        Task<IEnumerable<PointOfInterest?>> GetPointsOfInterestForCityAsync(int cityId);

        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);

        // deleting doesn't require async as it's just marking the entity for deletion, the actual delete happens when SaveChangesAsync is called
        void DeletePointOfInterest(PointOfInterest pointOfInterest);

        // save changes to the database, returns true if changes were saved successfully, false otherwise
        Task<bool> SaveChangesAsync();
    }
}
