using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MagicVilla_VillaAPI.DATA
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
        public DbSet<Villa> Villas { get; set; } //Villas: Name of tabel in database
        public DbSet<VillaNumber > VillaNumbers { get; set; }
        public DbSet<LocalUser> LocalUser { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa
                {
                    Id = 1,
                    Name = "Royal Villa",
                    Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa3.jpg",
                    Occupancy = 4,
                    Rate = 200,
                    Sqft = 550,
                    Amenity = "",
                    CreateDate = DateTime.Now
                },
              new Villa
              {
                  Id = 2,
                  Name = "Premium Pool Villa",
                  Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                  ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa1.jpg",
                  Occupancy = 4,
                  Rate = 300,
                  Sqft = 550,
                  Amenity = "",
                  CreateDate = DateTime.Now
              },
              new Villa
              {
                  Id = 3,
                  Name = "Luxury Pool Villa",
                  Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                  ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa4.jpg",
                  Occupancy = 4,
                  Rate = 400,
                  Sqft = 750,
                  Amenity = "",
                  CreateDate = DateTime.Now
              },
              new Villa
              {
                  Id = 4,
                  Name = "Diamond Villa",
                  Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                  ImageUrl = "https://dotnetmastery/bluevillaimages/villa5.jpg",
                  Occupancy = 4,
                  Rate = 550,
                  Sqft = 900,
                  Amenity = "",
                  CreateDate = DateTime.Now
              },
              new Villa
              {
                  Id = 5,
                  Name = "Diamond Pool Villa",
                  Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                  ImageUrl = "https://dotnetmastery/bluevillaimages/villa6.jpg",
                  Occupancy = 4,
                  Rate = 600,
                  Sqft = 1100,
                  Amenity = "",
                  CreateDate = DateTime.Now
              });

            modelBuilder.Entity<VillaNumber>().HasData(
                new VillaNumber
                {
                    VillaNo = 1,
                    SpecialDetails = "sea view",
                    CreateDate = DateTime.Now
                },

                new VillaNumber
                {
                    VillaNo = 2,
                    SpecialDetails = "garden view",
                    CreateDate = DateTime.Now
                },

                new VillaNumber
                {
                    VillaNo = 3,
                    SpecialDetails = "wood in the garden",
                    CreateDate = DateTime.Now
                }

            );

            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    Name = "Jacopo",
                    Surname = "Grazioli",
                    Age = 31
                },
                new Customer
                {
                    Id = 2,
                    Name = "Paolo",
                    Surname = "Rossi",
                    Age = 35
                },
                new Customer
                {
                    Id = 3,
                    Name = "Franco",
                    Surname = "Bianchi",
                    Age = 23
                }
            );

        }
    }
}
