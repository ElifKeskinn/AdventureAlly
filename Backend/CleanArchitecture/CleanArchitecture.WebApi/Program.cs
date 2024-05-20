using CleanArchitecture.Core;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Infrastructure.Models;
using CleanArchitecture.WebApi.Configuration;
using CleanArchitecture.WebApi.Controllers;
using CleanArchitecture.WebApi.Extensions;
using CleanArchitecture.WebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Net.Http;
using MongoDB.Driver;
using CleanArchitecture.Infrastructure.Services;
using CleanArchitecture.Core.Entities;





var builder = WebApplication.CreateBuilder(args);


//Add configurations
builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true);
builder.Services.AddScoped<GoogleMapsService>();


// Add services to the container.

builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceInfrastructure(builder.Configuration);

builder.Services.AddScoped<MongoDbContext>(serviceProvider =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDB");
    return new MongoDbContext(connectionString);
});

builder.Services.AddScoped<MongoDBService<BaseEntity>, MongoDBService<BaseEntity>>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDB");
    var databaseName = "database1";
    var collectionName = "BaseEntity";
    return new MongoDBService<BaseEntity>(connectionString, databaseName, collectionName);
});

builder.Services.AddScoped<MongoDBService<Deal>, MongoDBService<Deal>>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDB");
    var databaseName = "database1";
    var collectionName = "Deal";
    return new MongoDBService<Deal>(connectionString, databaseName, collectionName);
});

builder.Services.AddScoped<MongoDBService<DealValidity>, MongoDBService<DealValidity>>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDB");
    var databaseName = "database1";
    var collectionName = "DealValidity";
    return new MongoDBService<DealValidity>(connectionString, databaseName, collectionName);
});


builder.Services.AddScoped<MongoDBService<Interests>, MongoDBService<Interests>>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDB");
    var databaseName = "database1";
    var collectionName = "Interests";
    return new MongoDBService<Interests>(connectionString, databaseName, collectionName);
});
builder.Services.AddScoped<MongoDBService<LocalBusiness>, MongoDBService<LocalBusiness>>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDB");
    var databaseName = "database1";
    var collectionName = "LocalBusiness";
    return new MongoDBService<LocalBusiness>(connectionString, databaseName, collectionName);
});
builder.Services.AddScoped<MongoDBService<NotificationPreferences>, MongoDBService<NotificationPreferences>>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDB");
    var databaseName = "database1";
    var collectionName = "NotificationPreferences";
    return new MongoDBService<NotificationPreferences>(connectionString, databaseName, collectionName);
});
builder.Services.AddScoped<MongoDBService<TourPackage>, MongoDBService<TourPackage>>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDB");
    var databaseName = "database1";
    var collectionName = "TourPackage";
    return new MongoDBService<TourPackage>(connectionString, databaseName, collectionName);
});
builder.Services.AddScoped<MongoDBService<User>, MongoDBService<User>>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDB");
    var databaseName = "database1";
    var collectionName = "User";
    return new MongoDBService<User>(connectionString, databaseName, collectionName);
});
builder.Services.AddScoped<MongoDBService<UserPreferences>, MongoDBService<UserPreferences>>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDB");
    var databaseName = "database1";
    var collectionName = "UserPreferences";
    return new MongoDBService<UserPreferences>(connectionString, databaseName, collectionName);
});


builder.Services.AddSwaggerExtension();
builder.Services.AddControllers();
builder.Services.AddApiVersioningExtension();
builder.Services.AddHealthChecks();
builder.Services.AddScoped<IUpdateUserService, UpdateUserService>();
builder.Services.AddScoped<UpdateUserController>();
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
builder.Services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
builder.Services.AddHttpClient<GoogleMapsService>();
var configuration = builder.Configuration;

// Daha sonra configuration nesnesini kullanabilirsiniz
var weatherApiKey = configuration.GetValue<string>("ApiSettings:ApiKey");

builder.Services.AddScoped<WeatherService>(serviceProvider =>
{
    var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>();
    return new WeatherService(serviceProvider.GetRequiredService<IHttpClientFactory>(), apiSettings);
});

//Build the application
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseRouting();
app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseAuthentication();
app.UseAuthorization();
app.UseSwaggerExtension();
app.UseErrorHandlingMiddleware();
app.UseHealthChecks("/health");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


//Initialize Logger

Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(app.Configuration)
                .CreateLogger();

//Seed Default Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await CleanArchitecture.Infrastructure.Seeds.DefaultRoles.SeedAsync(userManager, roleManager);
        await CleanArchitecture.Infrastructure.Seeds.DefaultSuperAdmin.SeedAsync(userManager, roleManager);
        await CleanArchitecture.Infrastructure.Seeds.DefaultBasicUser.SeedAsync(userManager, roleManager);
        Log.Information("Finished Seeding Default Data");
        Log.Information("Application Starting");
    }
    catch (Exception ex)
    {
        Log.Warning(ex, "An error occurred seeding the DB");
    }
    finally
    {
        Log.CloseAndFlush();
    }
}

//Start the application
app.Run();