using MongoDB.Driver;

namespace CleanArchitecture.Infrastructure.Contexts
{
    public interface IMongoDbContext
    {
        IMongoCollection<T> GetCollection<T>(string collectionName);
        IMongoCollection<T> GetCollection<T>();
    }
}
