"use client";

import { Package, DollarSign, AlertCircle, FolderTree } from "lucide-react";
import { StatsCard } from "@/components/dashboard/stats-card";
import { CategoryChart } from "@/components/dashboard/category-chart";
import { LowStockTable } from "@/components/dashboard/low-stock-table";
import { useDashboard } from "@/app/hooks/use-dashboard";
import { formatCurrency } from "@/lib/utils";
import { Skeleton } from "@/components/ui/skeleton";

export default function DashboardPage() {
  const { stats, lowStockProducts, isLoading } = useDashboard();

  if (isLoading) {
    return (
      <div className="space-y-6">
        <div>
          <h1 className="text-3xl font-bold">Dashboard</h1>
          <p className="text-muted-foreground">
            Overview of your inventory and sales
          </p>
        </div>

        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
          {[...Array(4)].map((_, i) => (
            <Skeleton key={i} className="h-32" />
          ))}
        </div>

        <div className="grid gap-4 lg:grid-cols-2">
          <Skeleton className="h-96" />
          <Skeleton className="h-96" />
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Header */}
      <div>
        <h1 className="text-3xl font-bold">Dashboard</h1>
        <p className="text-muted-foreground">
          Overview of your inventory and sales
        </p>
      </div>

      {/* Stats Cards */}
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
        <StatsCard
          title="Total Products"
          value={stats?.totalProducts || 0}
          icon={Package}
          description="Active products in inventory"
        />
        <StatsCard
          title="Inventory Value"
          value={formatCurrency(stats?.totalInventoryValue || 0)}
          icon={DollarSign}
          description="Total value of stock"
        />
        <StatsCard
          title="Low Stock Items"
          value={stats?.lowStockCount || 0}
          icon={AlertCircle}
          description="Products below 10 units"
        />
        <StatsCard
          title="Categories"
          value={stats?.productsByCategory.length || 0}
          icon={FolderTree}
          description="Active categories"
        />
      </div>

      {/* Charts and Tables */}
      <div className="grid gap-4 lg:grid-cols-2">
        <CategoryChart data={stats?.productsByCategory || []} />
        <LowStockTable products={lowStockProducts} />
      </div>
    </div>
  );
}