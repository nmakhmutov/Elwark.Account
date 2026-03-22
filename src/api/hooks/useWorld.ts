import { useQuery } from '@tanstack/react-query';
import { useApiClient } from '../ApiClientProvider';
import type { ApiError, Country, Timezone } from '../types';

export function useCountries() {
  const client = useApiClient();

  return useQuery<Country[], ApiError>({
    queryKey: ['countries'],
    queryFn: async () => {
      const { data } = await client.get<Country[]>('countries');
      return data;
    },
    staleTime: Infinity,
  });
}

export function useTimezones() {
  const client = useApiClient();

  return useQuery<Timezone[], ApiError>({
    queryKey: ['timezones'],
    queryFn: async () => {
      const { data } = await client.get<Timezone[]>('timezones');
      return data;
    },
    staleTime: Infinity,
  });
}
