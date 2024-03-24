using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using SolarWatch.Model;

namespace SolarWatch.Context;

public class SolarWatchContext : DbContext
{
    public DbSet<LocationData> LocationDatas { get; set; }
    public DbSet<CityInformation> CityInformations { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Env.Load();
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        optionsBuilder.UseSqlServer(
            connectionString);
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<LocationData>()
            .HasIndex(u => u.City)
            .IsUnique();
    
        builder.Entity<LocationData>()
            .HasData(
                new LocationData { City = "London", Lat = 51.509865, Lon = -0.118092 },
                new LocationData { City = "Budapest", Lat = 47.497913, Lon = 19.040236 },
                new LocationData { City = "Paris", Lat = 48.864716, Lon = 2.349014 }
            );
    }
}