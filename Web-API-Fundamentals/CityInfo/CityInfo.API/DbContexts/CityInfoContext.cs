using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts
{
    // use DbContext to show this class will be used to interact with the database
    public class CityInfoContext : DbContext
    {
        public CityInfoContext(DbContextOptions<CityInfoContext> options)
            : base(options) { }

        //add model builder to allow us to create objects from our model and seed the database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // use the model builder to seed the database with initial data
            modelBuilder
                .Entity<City>()
                .HasData(
                    new City("New York City")
                    {
                        Id = 1,
                        Description = "The one with that big park.",
                    },
                    new City("Antwerp")
                    {
                        Id = 2,
                        Description = "The one with the cathedral that was never really finished.",
                    },
                    new City("Oslo") { Id = 3, Description = "The capital of Norway." }
                );
            modelBuilder
                .Entity<PointOfInterest>()
                .HasData(
                    new PointOfInterest("Central Park")
                    {
                        Id = 1,
                        CityId = 1,
                        Description = "A large public park in New York City.",
                    },
                    new PointOfInterest("Metropolitan Museum of Art")
                    {
                        Id = 2,
                        CityId = 1,
                        Description = "An art museum located on the eastern edge of Central Park.",
                    },
                    new PointOfInterest("Cathedral of Our Lady")
                    {
                        Id = 3,
                        CityId = 2,
                        Description = "A Roman Catholic cathedral in Antwerp, Belgium.",
                    },
                    new PointOfInterest("Vigeland Sculpture Park")
                    {
                        Id = 4,
                        CityId = 3,
                        Description =
                            "A park in Oslo featuring over 200 sculptures by Gustav Vigeland.",
                    }
                );

            base.OnModelCreating(modelBuilder);
        }

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
