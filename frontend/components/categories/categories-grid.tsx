"use client";

import { Card, CardContent, CardFooter, CardHeader } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Pencil, Trash2, FolderTree } from "lucide-react";
import type { Category } from "@/app/types";
import { formatDateTime } from "@/lib/utils";
import { useAuthStore } from "@/store/auth-store";

interface CategoriesGridProps {
  categories: Category[];
  onEdit: (category: Category) => void;
  onDelete: (category: Category) => void;
}

export function CategoriesGrid({
  categories,
  onEdit,
  onDelete,
}: CategoriesGridProps) {
  const hasRole = useAuthStore((state) => state.hasRole);
  const canEdit = hasRole("Admin") || hasRole("Manager");
  const canDelete = hasRole("Admin");

  if (categories.length === 0) {
    return (
      <div className="flex h-64 items-center justify-center border rounded-lg">
        <div className="text-center">
          <FolderTree className="mx-auto h-12 w-12 text-muted-foreground mb-4" />
          <p className="text-muted-foreground">No categories found</p>
          <p className="text-sm text-muted-foreground mt-1">
            Create your first category to get started
          </p>
        </div>
      </div>
    );
  }

  return (
    <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
      {categories.map((category) => (
        <Card key={category.id}>
          <CardHeader>
            <div className="flex items-start justify-between">
              <div className="flex items-center space-x-2">
                <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-primary/10">
                  <FolderTree className="h-5 w-5 text-primary" />
                </div>
                <div>
                  <h3 className="font-semibold">{category.name}</h3>
                  <p className="text-xs text-muted-foreground">
                    {formatDateTime(category.createdAt)}
                  </p>
                </div>
              </div>
            </div>
          </CardHeader>
          <CardContent>
            <p className="text-sm text-muted-foreground line-clamp-2">
              {category.description}
            </p>
          </CardContent>
          {(canEdit || canDelete) && (
            <CardFooter className="flex space-x-2">
              {canEdit && (
                <Button
                  variant="outline"
                  size="sm"
                  onClick={() => onEdit(category)}
                  className="flex-1"
                >
                  <Pencil className="mr-2 h-4 w-4" />
                  Edit
                </Button>
              )}
              {canDelete && (
                <Button
                  variant="outline"
                  size="sm"
                  onClick={() => onDelete(category)}
                  className="flex-1 text-destructive hover:bg-destructive hover:text-destructive-foreground"
                >
                  <Trash2 className="mr-2 h-4 w-4" />
                  Delete
                </Button>
              )}
            </CardFooter>
          )}
        </Card>
      ))}
    </div>
  );
}
