using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Core.Wrappers;
using CleanArchitecture.Core.Settings;
using CleanArchitecture.Infrastructure.Contexts;
using CleanArchitecture.Infrastructure.Models;
using CleanArchitecture.Infrastructure.Repositories;
using CleanArchitecture.Infrastructure.Repository;
using CleanArchitecture.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Newtonsoft.Json;
using AspNetCore.Identity.Mongo;

using System;
using System.Text;
using ApplicationUser = CleanArchitecture.Infrastructure.Models.ApplicationUser;
using ApplicationRole = CleanArchitecture.Infrastructure.Models.ApplicationRole;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository;
//using MongoDbContext = CleanArchitecture.Infrastructure.Contexts.MongoDbContext;
using MongoDB.Driver.Core.Configuration;
using MongoDbContext = MongoDbGenericRepository.MongoDbContext;

namespace CleanArchitecture.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {

            }
            else if (configuration.GetValue<bool>("UseMongoDatabase"))
            {// MongoDB bağlantı dizesini alıyoruz
                try
                {
                    var mongoConnectionString = configuration.GetConnectionString("ConnectionString");
                    var mongoDatabaseName = configuration.GetValue<string>("DatabaseName");

                    if (string.IsNullOrEmpty(mongoConnectionString))
                    {
                        throw new ArgumentException("MongoDB connection string is null or empty.");
                    }

                    if (string.IsNullOrEmpty(mongoDatabaseName))
                    {
                        throw new ArgumentException("MongoDB database name is null or empty.");
                    }
                    // MongoClientSettings ile bağlantıyı yapılandırıyoruz
                    var mongoClientSettings = MongoClientSettings.FromConnectionString(mongoConnectionString);

                    // MongoClient'i oluşturuyoruz
                    var mongoClient = new MongoClient(mongoClientSettings);

                    // IMongoDatabase'i oluşturuyoruz
                    var mongoDatabase = mongoClient.GetDatabase(mongoDatabaseName);
                    // IMongoDatabase'i DI konteynerına ekliyoruz
                    services.AddScoped<MongoDbContext>();
                    services.AddSingleton(mongoClient);
                    services.AddSingleton(mongoDatabase);
                    services.AddSingleton<IMongoClient>(mongoClient);
                    services.AddScoped<IMongoDatabase>(_ => mongoDatabase);
                    var mongoDbContext = new MongoDbContext(mongoConnectionString, mongoDatabaseName);
               

                    // Eğer özel bir ApplicationDbContext sınıfınız varsa, onu da hizmet olarak ekleyebilirsiniz
                    services.AddTransient<ApplicationDbContext>();

                    //services.AddIdentity<MongoIdentityUser, MongoIdentityRole>()
                    //    .AddMongoDbStores<MongoDbContext>(mongoDbContext)
                    //    .AddDefaultTokenProviders();

                  



                }
                catch (Exception ex)
                {
                    // Hatanın kaynağını bulmak için hata mesajını yazdırın
                    Console.WriteLine($"Error configuring MongoDB: {ex.Message}");
                    throw;
                }
            }


             //   #region Services
                services.AddScoped<IAccountService, AccountService>();
           // #endregion

            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JWTSettings:Issuer"],
                    ValidAudience = configuration["JWTSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                };
                o.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync(c.Exception.ToString());
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new Response<string>("You are not Authorized"));
                        return context.Response.WriteAsync(result);
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new Response<string>("You are not authorized to access this resource"));
                        return context.Response.WriteAsync(result);
                    },
                };
            });

            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IEmailService, EmailService>();

            #region Repositories
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddScoped<IUserRepositoryAsync, UserRepositoryAsync>();
            #endregion
        }
    }
}
