using MongoDB.Driver;
using MongoDbGenericRepository.Attributes;
using System;
using System.Linq;

namespace CleanArchitecture.Infrastructure.Contexts
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            var collectionName = (typeof(T).GetCustomAttributes(typeof(CollectionNameAttribute), true)
                .FirstOrDefault() as CollectionNameAttribute)?.Name;

            return _database.GetCollection<T>(collectionName);
        }
    }
}
