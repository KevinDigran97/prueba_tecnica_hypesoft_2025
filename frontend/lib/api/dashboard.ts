import { apiClient } from './client';
import type { DashboardStats } from '@/app/types';

export const dashboardApi = {
  getStats: async (): Promise<DashboardStats> => {
    const { data } = await apiClient.get('/dashboard/stats');
    return data;
  },
};