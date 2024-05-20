using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services

{
	public class MongoDBService<T>
	{
		private readonly IMongoCollection<T> _collection;

        public MongoDBService(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<T>(collectionName);
        }


        public async Task AddAsync(T document)
		{
			await _collection.InsertOneAsync(document);
		}

		public async Task<T> FindAsync(FilterDefinition<T> filter)
		{
			return await _collection.Find(filter).FirstOrDefaultAsync();
		}

		public async Task UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update)
		{
			await _collection.UpdateOneAsync(filter, update);
		}

		public async Task DeleteAsync(FilterDefinition<T> filter)
		{
			await _collection.DeleteOneAsync(filter);
		}
	}
}
