import { useQuery } from "@tanstack/react-query";
import { dashboardApi } from "@/lib/api/dashboard";
import { productsApi } from "@/lib/api/products";

export function useDashboard() {
  const statsQuery = useQuery({
    queryKey: ["dashboard", "stats"],
    queryFn: dashboardApi.getStats,
  });

  const lowStockQuery = useQuery({
    queryKey: ["products", "low-stock"],
    queryFn: productsApi.getLowStock,
  });

  return {
    stats: statsQuery.data,
    lowStockProducts: lowStockQuery.data || [],
    isLoading: statsQuery.isLoading || lowStockQuery.isLoading,
    error: statsQuery.error || lowStockQuery.error,
  };
}
