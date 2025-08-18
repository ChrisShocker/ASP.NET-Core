using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.API.Entities
{
    public class City
    {
        //The key decorator isn't required if the property is named Id or <ClassName>Id
        // but it's good practice to use it for clarity
        [Key]
        // ensure a new id is generated for each new instance
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        public ICollection<PointOfInterest> PointsOfInterest { get; set; } =
            new List<PointOfInterest>();

        public City(string name)
        {
            Name = name;
        }
    }
}
