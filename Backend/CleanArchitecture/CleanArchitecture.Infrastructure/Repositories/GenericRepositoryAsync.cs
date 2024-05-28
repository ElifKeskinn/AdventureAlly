using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Infrastructure.Contexts;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace CleanArchitecture.Infrastructure.Repository
{
    public class GenericRepositoryAsync<T> : IGenericRepositoryAsync<T> where T : AuditableBaseEntity
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepositoryAsync(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T> GetByIdAsync(ObjectId id)
        {
            var filter = Builders<T>.Filter.Eq(e => e.Id, id);
            return await _dbContext.GetCollection<T>().Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize)
        {
            return await _dbContext.GetCollection<T>()
                .Find(Builders<T>.Filter.Empty)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.AddAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            await _dbContext.UpdateAsync(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            var filter = Builders<T>.Filter.Eq(e => e.Id, entity.Id);
            await _dbContext.GetCollection<T>().DeleteOneAsync(filter);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.GetCollection<T>()
                .Find(Builders<T>.Filter.Empty)
                .ToListAsync();
        }
    }
}
