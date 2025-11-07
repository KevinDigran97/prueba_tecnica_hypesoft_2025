export interface Product {
    id: string;
    name: string;
    description: string;
    price: number;
    categoryId: string;
    categoryName?: string;
    stock: number;
    isLowStock: boolean;
    createdAt: string;
    updatedAt?: string;
  }
  
  export interface Category {
    id: string;
    name: string;
    description: string;
    createdAt: string;
    updatedAt?: string;
  }
  
  export interface PagedResult<T> {
    items: T[];
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
  }
  
  export interface DashboardStats {
    totalProducts: number;
    totalInventoryValue: number;
    lowStockCount: number;
    productsByCategory: CategoryStats[];
  }
  
  export interface CategoryStats {
    categoryId: string;
    categoryName: string;
    productCount: number;
  }
  
  export interface CreateProductDto {
    name: string;
    description: string;
    price: number;
    categoryId: string;
    stock: number;
  }
  
  export interface UpdateProductDto extends CreateProductDto {
    id: string;
  }
  
  export interface CreateCategoryDto {
    name: string;
    description: string;
  }
  
  export interface UpdateCategoryDto extends CreateCategoryDto {
    id: string;
  }
  
  export interface User {
    id: string;
    username: string;
    email: string;
    roles: string[];
  }