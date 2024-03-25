using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SolarWatch.Context;

using Microsoft.EntityFrameworkCore;
using Model;

public class SolarWatchContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public DbSet<LocationData> LocationDatas { get; set; }
    public DbSet<CityInformation> CityInformations { get; set; }
    
    public SolarWatchContext(DbContextOptions<SolarWatchContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<LocationData>()
            .HasIndex(u => u.City)
            .IsUnique();
    
        builder.Entity<LocationData>()
            .HasData(
                new LocationData { Id = 1, City = "London", Lat = 51.509865, Lon = -0.118092 },
                new LocationData { Id = 2, City = "Budapest", Lat = 47.497913, Lon = 19.040236 },
                new LocationData { Id = 3, City = "Paris", Lat = 48.864716, Lon = 2.349014 }
            );
    }
}