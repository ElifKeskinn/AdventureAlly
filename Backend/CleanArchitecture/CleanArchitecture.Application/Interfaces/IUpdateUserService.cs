using CleanArchitecture.Core.DTOs.User;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IUpdateUserService
    {
        Task<UserProfile> UpdateUserProfile(UserProfileUpdateRequest request);
    }
}
