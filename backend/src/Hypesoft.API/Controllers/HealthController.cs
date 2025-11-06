namespace Hypesoft.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Hypesoft.Infrastructure.Data;
using Hypesoft.Domain.Entities;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly MongoDbContext _dbContext;

    public HealthController(MongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Health check endpoint
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            // Test MongoDB connection
            await _dbContext.Products.CountDocumentsAsync(Builders<Product>.Filter.Empty);

            return Ok(new
            {
                status = "Healthy",
                timestamp = DateTime.UtcNow,
                services = new
                {
                    database = "Connected"
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(503, new
            {
                status = "Unhealthy",
                timestamp = DateTime.UtcNow,
                error = ex.Message
            });
        }
    }
}