import { apiClient } from './client';
import type { Product, PagedResult, CreateProductDto, UpdateProductDto } from '@/app/types';

export const productsApi = {
    getAll: async (params?: {
      page?: number;
      pageSize?: number;
      search?: string;
      categoryId?: string;
    }): Promise<PagedResult<Product>> => {
      const { data } = await apiClient.get('/products', { params });
      return data;
    },
  
    getById: async (id: string): Promise<Product> => {
      const { data } = await apiClient.get(`/products/${id}`);
      return data;
    },
  
    getLowStock: async (): Promise<Product[]> => {
      const { data } = await apiClient.get('/products/low-stock');
      return data;
    },
  
    create: async (product: CreateProductDto): Promise<Product> => {
      const { data } = await apiClient.post('/products', product);
      return data;
    },
  
    update: async (id: string, product: UpdateProductDto): Promise<Product> => {
      const { data } = await apiClient.put(`/products/${id}`, product);
      return data;
    },
  
    delete: async (id: string): Promise<void> => {
      await apiClient.delete(`/products/${id}`);
    },
  };