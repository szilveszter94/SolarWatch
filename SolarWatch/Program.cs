using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SolarWatch.Context;
using SolarWatch.Repository;
using SolarWatch.Service.Authentication;
using SolarWatch.Service.Processors;
using SolarWatch.Service.Providers;
using SolarWatch.Service.Token;

var builder = WebApplication.CreateBuilder(args);

var userSecrets = new Dictionary<string, string>
{
    { "validIssuer", builder.Configuration["JwtSettings:ValidIssuer"] },
    { "validAudience", builder.Configuration["JwtSettings:ValidAudience"] },
    { "issuerSigningKey", builder.Configuration["JwtSettings:IssuerSigningKey"] },
    { "adminEmail", builder.Configuration["AdminInfo:AdminEmail"]},
    { "adminPassword", builder.Configuration["AdminInfo:AdminPassword"]}
};

AddServices();
ConfigureSwagger();
AddDbContext();
AddAuthentication();
AddIdentity();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var authenticationSeeder = scope.ServiceProvider.GetRequiredService<AuthenticationSeeder>();
authenticationSeeder.AddRoles();
authenticationSeeder.AddAdmin();

app.Run();

void AddServices()
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddScoped<ILocationDataProvider, LatLongApi>();
    builder.Services.AddScoped<IWeatherDataProvider, SunsetSunriseApi>();
    builder.Services.AddScoped<ILocationDataProcessor, LocationDataProcessor>();
    builder.Services.AddScoped<ISunsetSunriseDataProcessor, SunsetSunriseDataProcessor>();
    builder.Services.AddScoped<ICityRepository, CityRepository>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ITokenService>(provider =>
        new TokenService(userSecrets["validIssuer"], userSecrets["validAudience"], userSecrets["issuerSigningKey"]));
    builder.Services.AddScoped<AuthenticationSeeder>(provider =>
    {
        var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = provider.GetRequiredService<UserManager<IdentityUser>>();
        
        var adminInfo = new Dictionary<string, string>
        {
            {"adminEmail", userSecrets["adminEmail"]},
            {"adminPassword", userSecrets["adminPassword"]}
        };

        return new AuthenticationSeeder(roleManager, userManager, adminInfo);
    });
}

void ConfigureSwagger()
{
    builder.Services.AddSwaggerGen(option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });
    });
}

void AddAuthentication()
{
    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = userSecrets["validIssuer"],
                ValidAudience = userSecrets["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(userSecrets["issuerSigningKey"])
                ),
            };
        });
}

void AddIdentity()
{
    builder.Services
        .AddIdentityCore<IdentityUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<SolarWatchContext>();
}

void AddDbContext()
{
    Env.Load();
    var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
    builder.Services.AddDbContext<SolarWatchContext>(options =>
    {
        Console.WriteLine("Trying to connect to database...");
        options.UseSqlServer(connectionString);
        Console.WriteLine("Connected to database!");
    });
}

