namespace Hypesoft.Infrastructure;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Hypesoft.Domain.Repositories;
using Hypesoft.Infrastructure.Data;
using Hypesoft.Infrastructure.Repositories;
using Hypesoft.Infrastructure.Repositories.InMemory;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration,
        bool isDevelopment = false)
    {
        // MongoDB Configuration
        services.Configure<MongoDbSettings>(
            configuration.GetSection("MongoDbSettings"));

        try 
        {
            var mongoSettings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
            if (string.IsNullOrEmpty(mongoSettings?.ConnectionString) || string.IsNullOrEmpty(mongoSettings?.DatabaseName))
            {
                throw new Exception("MongoDB configuration is missing or invalid");
            }

            services.AddSingleton<MongoDbContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
        }
        catch (Exception ex) when (isDevelopment)
        {
            // In development, we'll use in-memory repositories if MongoDB is not available
            services.AddScoped<IProductRepository, InMemoryProductRepository>();
            services.AddScoped<ICategoryRepository, InMemoryCategoryRepository>();
            Console.WriteLine($"Warning: Using in-memory repositories. MongoDB error: {ex.Message}");
        }
        catch (Exception ex)
        {
            // In production, we want to fail fast if MongoDB is not available
            throw new Exception("Failed to initialize MongoDB. Please check your configuration and ensure MongoDB is running.", ex);
        }

        return services;
    }

    public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider, bool isDevelopment = false)
    {
        try
        {
            var context = serviceProvider.GetService<MongoDbContext>();
            if (context != null)
            {
                await context.CreateIndexesAsync();
            }
            else if (!isDevelopment)
            {
                throw new InvalidOperationException("MongoDB context is not available");
            }
        }
        catch (Exception ex) when (isDevelopment)
        {
            Console.WriteLine($"Warning: Could not initialize database: {ex.Message}");
            // Continue with in-memory data in development
        }
    }
}