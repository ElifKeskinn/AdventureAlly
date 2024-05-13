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
            // Kullanýcý profili güncelleme iþlemini gerçekleþtirin, örneðin:
            var updatedProfile = await _userRepository.UpdateUserProfile(request);
            return updatedProfile;
        }
    }
}
