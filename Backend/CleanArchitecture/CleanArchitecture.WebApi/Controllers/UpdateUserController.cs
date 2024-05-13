using CleanArchitecture.Core.DTOs.User;
using CleanArchitecture.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CleanArchitecture.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateUserController : ControllerBase
    {
        private readonly IUpdateUserService _updateUserService;

        public UpdateUserController(IUpdateUserService updateUserService)
        {
            _updateUserService = updateUserService;
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileUpdateRequest request)
        {
            var updatedProfile = await _updateUserService.UpdateUserProfile(request);
            return Ok(updatedProfile);
        }
    }
}
