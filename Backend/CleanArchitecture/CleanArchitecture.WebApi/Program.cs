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
using Microsoft.AspNetCore.Identity.MongoDB;
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
using CleanArchitecture.Infrastructure.Contexts;
using CleanArchitecture.Infrastructure.Models;
using ApplicationUser = CleanArchitecture.Infrastructure.Models.ApplicationUser;
using AspNetCore.Identity.Mongo;
using MongoDB.Driver.Core.Configuration;
using IdentityRole = Microsoft.AspNetCore.Identity.MongoDB.IdentityRole;
using ApplicationRole = CleanArchitecture.Infrastructure.Models.ApplicationRole;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using MongoDbContext = CleanArchitecture.Infrastructure.Contexts.MongoDbContext;
using Org.BouncyCastle.Crypto.Tls;
using CleanArchitecture.Core.Settings;
using System.Configuration;




var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

//Add configurations
builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true);
builder.Services.AddScoped<GoogleMapsService>();


// Add services to the container.


builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceInfrastructure(builder.Configuration);

builder.Services.AddScoped(serviceProvider =>
{
    var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
    var databaseName = "AdventureAllyCluster"; // Veritabaný adýný buraya girin
    return new CleanArchitecture.Infrastructure.Contexts.MongoDbContext(connectionString, databaseName);
});
builder.Services.AddScoped(typeof(MongoDBService<>), typeof(MongoDBService<>));
builder.Services.AddScoped<UserService>();

//builder.Services.AddScoped<IAccountService, AccountService>();

//email services

builder.Services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
}).AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>(
    builder.Configuration.GetConnectionString("ConnectionString"), "AdvantureAllyCluster")
.AddDefaultTokenProviders();

/*
builder.Services.AddScoped<MongoDBService<Deal>, MongoDBService<Deal>>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDB");
    var databaseName = "AdventureAllyCluster";
    var collectionName = "Deal";
    return new MongoDBService<Deal>(connectionString, databaseName, collectionName);
});

builder.Services.AddScoped<MongoDBService<DealValidity>, MongoDBService<DealValidity>>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDB");
    var databaseName = "adventureAlly";
    var collectionName = "DealValidity";
    return new MongoDBService<DealValidity>(connectionString, databaseName, collectionName);
});


builder.Services.AddScoped<MongoDBService<Interests>, MongoDBService<Interests>>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDB");
    var databaseName = "adventureAlly";
    var collectionName = "Interests";
    return new MongoDBService<Interests>(connectionString, databaseName, collectionName);
});
builder.Services.AddScoped<MongoDBService<LocalBusiness>, MongoDBService<LocalBusiness>>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDB");
    var databaseName = "adventureAlly";
    var collectionName = "LocalBusiness";
    return new MongoDBService<LocalBusiness>(connectionString, databaseName, collectionName);
});
builder.Services.AddScoped<MongoDBService<NotificationPreferences>, MongoDBService<NotificationPreferences>>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDB");
    var databaseName = "adventureAlly";
    var collectionName = "NotificationPreferences";
    return new MongoDBService<NotificationPreferences>(connectionString, databaseName, collectionName);
});
builder.Services.AddScoped<MongoDBService<TourPackage>, MongoDBService<TourPackage>>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDB");
    var databaseName = "adventureAlly";
    var collectionName = "TourPackage";
    return new MongoDBService<TourPackage>(connectionString, databaseName, collectionName);
});
builder.Services.AddScoped<MongoDBService<User>, MongoDBService<User>>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDB");
    var databaseName = "adventureAlly";
    var collectionName = "User";
    return new MongoDBService<User>(connectionString, databaseName, collectionName);
});
builder.Services.AddScoped<MongoDBService<UserPreferences>, MongoDBService<UserPreferences>>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDB");
    var databaseName = "adventureAlly";
    var collectionName = "UserPreferences";
    return new MongoDBService<UserPreferences>(connectionString, databaseName, collectionName);
});
*/

builder.Services.AddScoped<UserService>();
builder.Services.AddSwaggerExtension();
builder.Services.AddControllers();
builder.Services.AddApiVersioningExtension();
builder.Services.AddHealthChecks();
builder.Services.AddScoped<IUpdateUserService, UpdateUserService>();
builder.Services.AddScoped<UpdateUserController>();
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
builder.Services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
builder.Services.AddHttpClient<GoogleMapsService>();
// Daha sonra configuration nesnesini kullanabilirsiniz
var weatherApiKey = configuration.GetValue<string>("ApiSettings:ApiKey");

builder.Services.AddScoped<WeatherService>(serviceProvider =>
{
    var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>();
    return new WeatherService(serviceProvider.GetRequiredService<IHttpClientFactory>(), apiSettings);
});

// Identity configuration

var options = new MongoIdentityOptions { 
UsersCollection = "Users",
RolesCollection = "Roles"
};

builder.Services.AddSingleton<MongoDbContext>(serviceProvider =>
{
    var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
    var databaseName = "AdventureAllyCluster"; // Veritabaný adýný buraya girin
    return new MongoDbContext(connectionString, databaseName);
});




var mongoConnectionString = builder.Configuration.GetConnectionString("ConnectionString");
var mongoDatabaseName = "AdventureAllyCluster"; // Veritabaný adýný buraya girin

builder.Services.Configure<MongoDbSettings>(options =>
{
    options.ConnectionString = mongoConnectionString;
    options.DatabaseName = mongoDatabaseName;
});


///AddDbContext<ApplicationDbContext>();
/*
builder.Services.AddIdentity<ApplicationUser, MongoIdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    // Diðer yapýlandýrmalar...
})
.AddMongoDbStores<ApplicationUser, MongoIdentityRole, Guid>(
    mongoConnectionString, mongoDatabaseName)
.AddDefaultTokenProviders();*/

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
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
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