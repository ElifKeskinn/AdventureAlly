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
            // Kullan�c� profili g�ncelleme i�lemini ger�ekle�tirin, �rne�in:
            var updatedProfile = await _userRepository.UpdateUserProfile(request);
            return updatedProfile;
        }
    }
}
