using CleanArchitecture.Core.DTOs.User;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Repositories;
using System.Threading.Tasks;

namespace CleanArchitecture.WebApi.Services
{
    public class UpdateUserService : IUpdateUserService
    {
        private readonly IUserRepositoryAsync _userRepository;

        public UpdateUserService(IUserRepositoryAsync userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserProfile> UpdateUserProfile(UserProfileUpdateRequest request)
        {
            // Kullanıcı profili güncelleme işlemini gerçekleştirin, örneğin:
            var updatedProfile = await _userRepository.UpdateUserProfile(request);
            return updatedProfile;
        }
    }
}
