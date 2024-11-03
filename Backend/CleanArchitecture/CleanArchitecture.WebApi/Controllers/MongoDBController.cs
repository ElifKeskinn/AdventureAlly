using CleanArchitecture.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MongoDBController<T> : ControllerBase
    {
        private readonly MongoDBService<T> _mongoDBService;

        public MongoDBController(MongoDBService<T> mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpPost]
        public async Task<IActionResult> AddDocument(T document)
        {
            try
            {
                await _mongoDBService.AddAsync(document);
                return Ok("Document added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocument(string id)
        {
            try
            {
                var document = await _mongoDBService.FindAsync(Builders<T>.Filter.Eq("_id", id));
                if (document != null)
                    return Ok(document);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(string id, T document)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq("_id", id);
                var update = Builders<T>.Update.Set("Name", "NewName"); // Örneðin: Belirli bir özelliði güncellemek için
                await _mongoDBService.UpdateAsync(filter, update);
                return Ok("Document updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(string id)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq("_id", id);
                await _mongoDBService.DeleteAsync(filter);
                return Ok("Document deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
