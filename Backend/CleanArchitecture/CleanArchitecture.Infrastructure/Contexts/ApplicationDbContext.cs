using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Entities;
using MongoDB.Driver;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Contexts
{
    public class ApplicationDbContext: DbContext
    {
        private readonly IMongoDatabase _database;
        private readonly IDateTimeService _dateTime;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public ApplicationDbContext(IMongoDatabase database, IDateTimeService dateTime, IAuthenticatedUserService authenticatedUser)
        {
            _database = database;
            _dateTime = dateTime;
            _authenticatedUser = authenticatedUser;
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Deal> Deals => _database.GetCollection<Deal>("Deals");
        public IMongoCollection<DealValidity> DealValidities => _database.GetCollection<DealValidity>("DealValidities");
        public IMongoCollection<LocalBusiness> LocalBusinesses => _database.GetCollection<LocalBusiness>("LocalBusinesses");
        public IMongoCollection<NotificationPreferences> NotificationPreferences => _database.GetCollection<NotificationPreferences>("NotificationPreferences");
        public IMongoCollection<TourPackage> TourPackages => _database.GetCollection<TourPackage>("TourPackages");
        public IMongoCollection<UserPreferences> UserPreferences => _database.GetCollection<UserPreferences>("UserPreferences");
        public IMongoCollection<Interests> Interests => _database.GetCollection<Interests>("Interests");

        // Veritabanındaki değişiklikleri kaydetmek için SaveChangesAsync metodu ekledik.
        public async Task<int> SaveChangesAsync()
        {
            // MongoDB'de SaveChangesAsync metoduna ihtiyaç yoktur, bu nedenle sadece 0 değerini döndürüyoruz.
            return 0;
        }

        // Veritabanındaki bir koleksiyona bir entity eklemek için AddAsync metodu eklendi.
        public async Task AddAsync<T>(T entity) where T : AuditableBaseEntity
        {
            entity.Created = _dateTime.NowUtc;
            entity.CreatedBy = _authenticatedUser.UserId;
            await GetCollection<T>().InsertOneAsync(entity);
        }

        // Veritabanındaki bir koleksiyonu güncellemek için UpdateAsync metodu eklendi.
        public async Task UpdateAsync<T>(T entity) where T : AuditableBaseEntity
        {
            entity.LastModified = _dateTime.NowUtc;
            entity.LastModifiedBy = _authenticatedUser.UserId;
            var filter = Builders<T>.Filter.Eq(e => e.Id, entity.Id);
            await GetCollection<T>().ReplaceOneAsync(filter, entity);
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            return _database.GetCollection<T>(typeof(T).Name);
        }
    }
}
