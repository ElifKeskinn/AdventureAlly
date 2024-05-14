using CleanArchitecture.Core.DTOs.User;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Infrastructure.Contexts;
using CleanArchitecture.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Repositories
{
    public class UserRepositoryAsync : GenericRepositoryAsync<User>, IUserRepositoryAsync
    {
        private readonly DbSet<User> _userss;

        public UserRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _userss = dbContext.Set<User>();
        }

        public Task<bool> IsUniqueEmailAsync(string email)
        {
            return _userss
                .AllAsync(p => p.Email != email);
        }

        public Task<UserProfile> UpdateUserProfile(UserProfileUpdateRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}
