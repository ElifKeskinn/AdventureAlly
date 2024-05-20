using CleanArchitecture.Core.Entities;
using CleanArchitecture.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealsController : ControllerBase
    {
        private readonly DealService _dealService;

        public DealsController(DealService dealService)
        {
            _dealService = dealService;
        }

        [HttpPost]
        public async Task<IActionResult> AddDealAsync(Deal deal)
        {
            await _dealService.AddDealAsync(deal);
            return Ok("Deal added");
        }

        [HttpGet]
        public async Task<IActionResult> GetDealAsync(string title)
        {
            var filter = Builders<Deal>.Filter.Eq(d => d.Title, title);
            var deal = await _dealService.FindDealAsync(filter);
            return Ok(deal);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDealAsync(string title, Deal updatedDeal)
        {
            var filter = Builders<Deal>.Filter.Eq(d => d.Title, title);
            var update = Builders<Deal>.Update.Set(d => d.Description, updatedDeal.Description);
            await _dealService.UpdateDealAsync(filter, update);
            return Ok("Deal updated");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDealAsync(string title)
        {
            var filter = Builders<Deal>.Filter.Eq(d => d.Title, title);
            await _dealService.DeleteDealAsync(filter);
            return Ok("Deal deleted");
        }
    }
}
