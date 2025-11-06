namespace Hypesoft.Application.DTOs;

public class DashboardStatsDto
{
    public int TotalProducts { get; set; }
    public decimal TotalInventoryValue { get; set; }
    public int LowStockCount { get; set; }
    public List<CategoryStatsDto> ProductsByCategory { get; set; } = new();
}

public class CategoryStatsDto
{
    public string CategoryId { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public int ProductCount { get; set; }
}