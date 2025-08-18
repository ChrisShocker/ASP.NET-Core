using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts
{
    // use DbContext to show this class will be used to interact with the database
    public class CityInfoContext : DbContext
    {
        public CityInfoContext(DbContextOptions<CityInfoContext> options)
            : base(options) { }

        //this injection should not be used and the above should be used instead and injected in the Program.cs
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("CityInfo.db");
        //    base.OnConfiguring(optionsBuilder);
        //}

        // Dbsets are used to query and save instances of the entity classes in the db
        // linq queries against the DbSet will be translated to query the the database
        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointsOfInterest { get; set; }
    }
}
