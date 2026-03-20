using AutoMapper;

namespace CityInfo.API.Profiles
{
    public class CityProfile: Profile
    {

        public CityProfile() {
            // create a mapping between the City entity and the CityWithoutPointsOfInterestDto, this will allow us to easily convert between the two types
            CreateMap<Entities.City, Models.CityWithoutPointsOfInterestDto>();

            // create a mapping between the City entity and the CityDto, this will allow us to easily convert between the two types
            CreateMap<Entities.City, Models.CityDto>();
        }
    }
}
