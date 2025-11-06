namespace Hypesoft.Application.Dashboard.GetDashboardStats;

using MediatR;
using Hypesoft.Domain.Repositories;
using Hypesoft.Application.DTOs;

public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, DashboardStatsDto>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public GetDashboardStatsQueryHandler(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<DashboardStatsDto> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        var totalProducts = await _productRepository.GetTotalCountAsync(cancellationToken);
        var totalValue = await _productRepository.GetTotalInventoryValueAsync(cancellationToken);
        var lowStockProducts = await _productRepository.GetLowStockAsync(cancellationToken);
        var allProducts = await _productRepository.GetAllAsync(cancellationToken);
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);

        var productsByCategory = allProducts
            .GroupBy(p => p.CategoryId)
            .Select(g => new CategoryStatsDto
            {
                CategoryId = g.Key,
                CategoryName = categories.FirstOrDefault(c => c.Id == g.Key)?.Name ?? "Unknown",
                ProductCount = g.Count()
            })
            .OrderByDescending(x => x.ProductCount)
            .ToList();

        return new DashboardStatsDto
        {
            TotalProducts = totalProducts,
            TotalInventoryValue = totalValue,
            LowStockCount = lowStockProducts.Count,
            ProductsByCategory = productsByCategory
        };
    }
}