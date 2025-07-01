namespace CityInfo.API.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        //property to calculate the number of points of interest
        public int NumberOfPointsOfInterest
        {
            get { return PointsOfInterest.Count; }
        }

        //instantiate the collection to avoid null reference exceptions
        public ICollection<PointOfInterestDto> PointsOfInterest { get; set; } =
            new List<PointOfInterestDto>();
    }
}
