using CleanArchitecture.Core.DTOs.User;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Infrastructure.Contexts;
using CleanArchitecture.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Repositories
{
    public class UserRepositoryAsync : GenericRepositoryAsync<User>, IUserRepositoryAsync
    {
        private readonly IMongoCollection<User> Users;

        public UserRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
           Users = dbContext.Users;
        }

        public async Task<bool> IsUniqueEmailAsync(string email)
        {
            var existingUser = await Users.Find(p => p.Email == email).FirstOrDefaultAsync();
            return existingUser == null;
        }

        public Task<UserProfile> UpdateUserProfile(UserProfileUpdateRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}
