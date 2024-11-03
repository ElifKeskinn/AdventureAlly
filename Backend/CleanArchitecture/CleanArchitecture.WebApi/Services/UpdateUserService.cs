using CleanArchitecture.Core.DTOs.User;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Repositories;
using System.Threading.Tasks;
using CleanArchitecture.Infrastructure.Contexts;

namespace CleanArchitecture.WebApi.Services
{
    public class UpdateUserService : IUpdateUserService
    {
        private readonly IUserRepositoryAsync _userRepository;
        private readonly MongoDbContext _dbContext; // MongoDB ba�lant�s�
        public UpdateUserService(IUserRepositoryAsync userRepository, MongoDbContext dbContext) // MongoDB ba�lant�s�n�n constructor'a eklenmesi
        {
            _userRepository = userRepository;
            _dbContext = dbContext;
        }
        public async Task<UserProfile> UpdateUserProfile(UserProfileUpdateRequest request)
        {
            // Kullan�c� profili g�ncelleme i�lemini ger�ekle�tirin, �rne�in:
            var updatedProfile = await _userRepository.UpdateUserProfile(request);
            return updatedProfile;
        }
    }
}
