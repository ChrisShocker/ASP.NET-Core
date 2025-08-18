using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.API.Entities
{
    public class PointOfInterest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        // decorator isn't required but good practice
        [ForeignKey("CityId")]
        // setup a one-to-many relationship with City
        // this will create a foreign key in the PointOfInterest table for the City
        public City? City { get; set; }

        // we can also explicitly define the foreign key property which is a good practice
        public int CityId { get; set; }

        public PointOfInterest(string name)
        {
            Name = name;
        }
    }
}
