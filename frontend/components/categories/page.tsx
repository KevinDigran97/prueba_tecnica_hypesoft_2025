"use client";

import { useState } from "react";
import { Plus } from "lucide-react";
import { Button } from "@/components/ui/button";
import { CategoriesGrid } from "@/components/categories/categories-grid";
import { CategoryDialog } from "@/components/categories/category-dialog";
import { DeleteCategoryDialog } from "@/components/categories/delete-category-dialog";
import { useCategories } from "@/app/hooks/use-categories";
import { useAuthStore } from "@/store/auth-store";
import type { Category } from "@/app/types";
import { Skeleton } from "@/components/ui/skeleton";

export default function CategoriesPage() {
  const [editingCategory, setEditingCategory] = useState<Category | undefined>();
  const [deletingCategory, setDeletingCategory] = useState<Category | undefined>();
  const [showCreateDialog, setShowCreateDialog] = useState(false);

  const hasRole = useAuthStore((state) => state.hasRole);
  const canCreate = hasRole("Admin") || hasRole("Manager");

  const { data: categories, isLoading } = useCategories();

  const handleEdit = (category: Category) => {
    setEditingCategory(category);
  };

  const handleDelete = (category: Category) => {
    setDeletingCategory(category);
  };

  if (isLoading) {
    return (
      <div className="space-y-6">
        <Skeleton className="h-10 w-64" />
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
          {[...Array(6)].map((_, i) => (
            <Skeleton key={i} className="h-48" />
          ))}
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">Categories</h1>
          <p className="text-muted-foreground">
            Organize your products into categories
          </p>
        </div>
        {canCreate && (
          <Button onClick={() => setShowCreateDialog(true)}>
            <Plus className="mr-2 h-4 w-4" />
            Add Category
          </Button>
        )}
      </div>


      <CategoriesGrid
        categories={categories || []}
        onEdit={handleEdit}
        onDelete={handleDelete}
      />


      <CategoryDialog
        open={showCreateDialog}
        onOpenChange={setShowCreateDialog}
      />

      <CategoryDialog
        category={editingCategory}
        open={!!editingCategory}
        onOpenChange={(open) => !open && setEditingCategory(undefined)}
      />

      {deletingCategory && (
        <DeleteCategoryDialog
          categoryId={deletingCategory.id}
          categoryName={deletingCategory.name}
          open={!!deletingCategory}
          onOpenChange={(open) => !open && setDeletingCategory(undefined)}
        />
      )}
    </div>
  );
}