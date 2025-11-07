import { apiClient } from './client';
import type { Category, CreateCategoryDto, UpdateCategoryDto } from '@/app/types';

export const categoriesApi = {
  getAll: async (): Promise<Category[]> => {
    const { data } = await apiClient.get('/categories');
    return data;
  },

  getById: async (id: string): Promise<Category> => {
    const { data } = await apiClient.get(`/categories/${id}`);
    return data;
  },

  create: async (category: CreateCategoryDto): Promise<Category> => {
    const { data } = await apiClient.post('/categories', category);
    return data;
  },

  update: async (id: string, category: UpdateCategoryDto): Promise<Category> => {
    const { data } = await apiClient.put(`/categories/${id}`, category);
    return data;
  },

  delete: async (id: string): Promise<void> => {
    await apiClient.delete(`/categories/${id}`);
  },
};