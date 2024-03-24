using SolarWatch.Context;
using SolarWatch.Repository;
using SolarWatch.Service.Processors;
using SolarWatch.Service.Providers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ILocationDataProvider, LatLongApi>();
builder.Services.AddScoped<IWeatherDataProvider, SunsetSunriseApi>();
builder.Services.AddScoped<ILocationDataProcessor, LocationDataProcessor>();
builder.Services.AddScoped<ISunsetSunriseDataProcessor, SunsetSunriseDataProcessor>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddDbContext<SolarWatchContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();