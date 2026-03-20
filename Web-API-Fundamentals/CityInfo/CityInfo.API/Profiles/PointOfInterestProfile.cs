using AutoMapper;

namespace CityInfo.API.Profiles
{
    public class PointOfInterestProfile: Profile
    {
        public PointOfInterestProfile()
        {
            // create a mapping between the PointOfInterest entity and the PointOfInterestDto
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
            CreateMap<Models.PointOfInterestForCreationDto, Entities.PointOfInterest>();
            CreateMap<Models.PointOfInterestUpdateDto, Entities.PointOfInterest>();
        }
    }
}
