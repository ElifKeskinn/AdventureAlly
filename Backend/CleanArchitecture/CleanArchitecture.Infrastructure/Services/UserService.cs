using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitecture.WebApi.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IOptions<MongoSettings> mongoSettings)
        {
            var mongoClient = new MongoClient(mongoSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
            _users = mongoDatabase.GetCollection<User>("Users");
        }

        public async Task<List<User>> GetAsync() =>
            await _users.Find(user => true).ToListAsync();

        public async Task<User> GetAsync(ObjectId id) =>
            await _users.Find<User>(user => user.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(User user) =>
            await _users.InsertOneAsync(user);

        public async Task UpdateAsync(ObjectId id, User updatedUser) =>
            await _users.ReplaceOneAsync(user => user.Id == id, updatedUser);

        public async Task RemoveAsync(ObjectId id) =>
            await _users.DeleteOneAsync(user => user.Id == id);
    }
}
