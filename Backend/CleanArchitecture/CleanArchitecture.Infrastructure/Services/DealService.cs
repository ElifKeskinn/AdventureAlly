using CleanArchitecture.Core.Entities;
using CleanArchitecture.Infrastructure.Services;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services

{
    public class DealService
    {
        private readonly MongoDBService<Deal> _mongoDBService;

        public DealService(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _mongoDBService = new MongoDBService<Deal>(connectionString, databaseName, collectionName);
        }

        public async Task AddDealAsync(Deal deal)
        {
            await _mongoDBService.AddAsync(deal);
        }

        public async Task<Deal> FindDealAsync(FilterDefinition<Deal> filter)
        {
            return await _mongoDBService.FindAsync(filter);
        }

        public async Task UpdateDealAsync(FilterDefinition<Deal> filter, UpdateDefinition<Deal> update)
        {
            await _mongoDBService.UpdateAsync(filter, update);
        }

        public async Task DeleteDealAsync(FilterDefinition<Deal> filter)
        {
            await _mongoDBService.DeleteAsync(filter);
        }
    }
}
