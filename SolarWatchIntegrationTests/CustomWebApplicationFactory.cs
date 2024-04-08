using System.Data.Common;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SolarWatch.Context;
using SolarWatch.Model;
using SolarWatchUnitTests.IntegrationTests.Services;

namespace SolarWatchUnitTests;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<SolarWatchContext>));
            services.Remove(dbContextDescriptor);
            
            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbConnection));
            services.Remove(dbConnectionDescriptor);

            services.RemoveAll<IAuthorizationPolicyProvider>();
            services.RemoveAll<IAuthorizationHandler>();
            
            services.AddAuthorization(opt =>
            {
                opt.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes("Test")
                    .Combine(opt.DefaultPolicy)
                    .Build();
            });
            
            services.AddSingleton<IAuthorizationHandler, FakeAuthorizationHandler>();
            services.AddDbContext<SolarWatchContext>(options =>
            {
                options.UseInMemoryDatabase("SolarWatchContext");
            });
            SeedTestData(services);
        });

        builder.UseEnvironment("Development");
        
        void SeedTestData(IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var context = serviceProvider.GetRequiredService<SolarWatchContext>();
            
            context.AutocompleteCityModels.Add(new AutocompleteCityModel() { Label = "Budapest", Id = 1 });
            context.AutocompleteCityModels.Add(new AutocompleteCityModel() { Label = "Debrecen", Id = 2 });
            context.LocationDatas.Add(new LocationData() { City = "Budapest", Id = 1, Lat = 47, Lon = 21 });
            context.CityInformations.Add(new CityInformation() { City = "Budapest", Id = 1, Date = new DateTime(2024,03, 31), Sunrise = "4 AM", Sunset = "6 PM"});
            context.SaveChanges();
        }
    }
}