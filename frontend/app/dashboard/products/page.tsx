// "use client";

// import { useState } from "react";
// import { Plus } from "lucide-react";
// import { Button } from "@/components/ui/button";
// import { Card, CardContent } from "@/components/ui/card";
// import { ProductsTable } from "@/components/products/products-table";
// import { ProductsFilters } from "@/components/products/products-filters";
// import { Pagination } from "@/components/products/pagination";
// import { ProductDialog } from "@/components/products/product-dialog";
// import { DeleteProductDialog } from "@/components/products/delete-product-dialog";
// import { useProducts } from "@/hooks/use-products";
// import { useCategories } from "@/hooks/use-categories";
// import { useAuthStore } from "@/store/auth-store";
// import type { Product } from "@/types";
// import { Skeleton } from "@/components/ui/skeleton";

// export default function ProductsPage() {
//   const [page, setPage] = useState(1);
//   const [search, setSearch] = useState("");
//   const [categoryId, setCategoryId] = useState("");
//   const [editingProduct, setEditingProduct] = useState<Product | undefined>();
//   const [deletingProduct, setDeletingProduct] = useState<Product | undefined>();
//   const [showCreateDialog, setShowCreateDialog] = useState(false);

//   const hasRole = useAuthStore((state) => state.hasRole);
//   const canCreate = hasRole("Admin") || hasRole("Manager");

//   const { data: productsData, isLoading } = useProducts({
//     page,
//     pageSize: 10,
//     search: search || undefined,
//     categoryId: categoryId || undefined,
//   });

//   const { data: categories } = useCategories();

//   const handleEdit = (product: Product) => {
//     setEditingProduct(product);
//   };

//   const handleDelete = (product: Product) => {
//     setDeletingProduct(product);
//   };

//   if (isLoading) {
//     return (
//       <div className="space-y-6">
//         <Skeleton className="h-10 w-64" />
//         <Skeleton className="h-12 w-full" />
//         <Skeleton className="h-96 w-full" />
//       </div>
//     );
//   }

//   return (
//     <div className="space-y-6">
//       {/* Header */}
//       <div className="flex items-center justify-between">
//         <div>
//           <h1 className="text-3xl font-bold">Products</h1>
//           <p className="text-muted-foreground">
//             Manage your product inventory
//           </p>
//         </div>
//         {canCreate && (
//           <Button onClick={() => setShowCreateDialog(true)}>
//             <Plus className="mr-2 h-4 w-4" />
//             Add Product
//           </Button>
//         )}
//       </div>

//       {/* Filters */}
//       <Card>
//         <CardContent className="pt-6">
//           <ProductsFilters
//             categories={categories || []}
//             onSearchChange={setSearch}
//             onCategoryChange={setCategoryId}
//           />
//         </CardContent>
//       </Card>

//       {/* Table */}
//       <ProductsTable
//         products={productsData?.items || []}
//         onEdit={handleEdit}
//         onDelete={handleDelete}
//       />

//       {/* Pagination */}
//       {productsData && productsData.totalPages > 1 && (
//         <Pagination
//           currentPage={productsData.page}
//           totalPages={productsData.totalPages}
//           onPageChange={setPage}
//         />
//       )}

//       {/* Dialogs */}
//       <ProductDialog
//         open={showCreateDialog}
//         onOpenChange={setShowCreateDialog}
//       />

//       <ProductDialog
//         product={editingProduct}
//         open={!!editingProduct}
//         onOpenChange={(open) => !open && setEditingProduct(undefined)}
//       />

//       {deletingProduct && (
//         <DeleteProductDialog
//           productId={deletingProduct.id}
//           productName={deletingProduct.name}
//           open={!!deletingProduct}
//           onOpenChange={(open) => !open && setDeletingProduct(undefined)}
//         />
//       )}
//     </div>
//   );
// }