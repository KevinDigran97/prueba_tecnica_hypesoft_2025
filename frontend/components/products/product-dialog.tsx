"use client";

import { useEffect } from "react";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { ProductForm, ProductFormData } from "./product-form";
import { useCreateProduct, useUpdateProduct } from "@/app/hooks/use-products";
import { useCategories } from "@/app/hooks/use-categories";
import { toast } from "sonner";
import type { Product } from "@/app/types";

interface ProductDialogProps {
  product?: Product;
  open: boolean;
  onOpenChange: (open: boolean) => void;
}

export function ProductDialog({ product, open, onOpenChange }: ProductDialogProps) {
  const { data: categories } = useCategories();
  const createMutation = useCreateProduct();
  const updateMutation = useUpdateProduct();

  const isLoading = createMutation.isPending || updateMutation.isPending;

  const handleSubmit = async (data: ProductFormData) => {
    try {
      if (product) {
        await updateMutation.mutateAsync({
          id: product.id,
          data: { ...data, id: product.id },
        });
        toast.success("Product updated successfully");
      } else {
        await createMutation.mutateAsync(data);
        toast.success("Product created successfully");
      }
      onOpenChange(false);
    } catch (error) {
      toast.error("An error occurred. Please try again.");
    }
  };


  useEffect(() => {
    if (!open) {
      createMutation.reset();
      updateMutation.reset();
    }
  }, [open, createMutation, updateMutation]);

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-2xl">
        <DialogHeader>
          <DialogTitle>
            {product ? "Edit Product" : "Create New Product"}
          </DialogTitle>
        </DialogHeader>
        <ProductForm
          product={product}
          categories={categories || []}
          onSubmit={handleSubmit}
          isLoading={isLoading}
        />
      </DialogContent>
    </Dialog>
  );
}