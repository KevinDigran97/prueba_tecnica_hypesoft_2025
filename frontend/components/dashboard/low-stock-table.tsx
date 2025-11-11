"use client";

import Link from "next/link";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { AlertCircle, ExternalLink } from "lucide-react";
import type { Product } from "@/app/types";
import { formatCurrency } from "@/lib/utils";

interface LowStockTableProps {
  products: Product[];
}

export function LowStockTable({ products }: LowStockTableProps) {
  if (products.length === 0) {
    return (
      <Card className="col-span-1 lg:col-span-2">
        <CardHeader>
          <CardTitle className="flex items-center">
            <AlertCircle className="mr-2 h-5 w-5 text-orange-500" />
            Low Stock Products
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="text-center text-muted-foreground py-8">
            <p>No products with low stock</p>
          </div>
        </CardContent>
      </Card>
    );
  }

  return (
    <Card className="col-span-1 lg:col-span-2">
      <CardHeader className="flex flex-row items-center justify-between">
        <CardTitle className="flex items-center">
          <AlertCircle className="mr-2 h-5 w-5 text-orange-500" />
          Low Stock Products
        </CardTitle>
        <Link href="/dashboard/products">
          <Button variant="ghost" size="sm">
            View All
            <ExternalLink className="ml-2 h-4 w-4" />
          </Button>
        </Link>
      </CardHeader>
      <CardContent>
        <div className="space-y-4">
          {products.slice(0, 5).map((product) => (
            <div
              key={product.id}
              className="flex items-center justify-between border-b pb-4 last:border-0 last:pb-0"
            >
              <div className="flex-1">
                <p className="font-medium">{product.name}</p>
                <p className="text-sm text-muted-foreground">
                  {product.categoryName} • {formatCurrency(product.price)}
                </p>
              </div>
              <Badge variant="destructive" className="ml-4">
                {product.stock} left
              </Badge>
            </div>
          ))}
        </div>
      </CardContent>
    </Card>
  );
}
