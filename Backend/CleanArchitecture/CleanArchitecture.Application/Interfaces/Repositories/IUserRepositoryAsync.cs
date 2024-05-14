using CleanArchitecture.Core.Entities;
using System.Threading.Tasks;
using CleanArchitecture.Core.DTOs.User;

namespace CleanArchitecture.Core.Interfaces.Repositories
{
    public interface IUserRepositoryAsync : IGenericRepositoryAsync<User>
    {
        Task<bool> IsUniqueEmailAsync(string email);
        Task<UserProfile> UpdateUserProfile(UserProfileUpdateRequest request);
    }
}
