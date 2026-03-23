import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { useApiClient } from '../ApiClientProvider';
import type {
  Account,
  ApiError,
  Confirming,
  Email,
  EmailRequest,
  UpdateRequest,
  VerifyRequest,
} from '../types';

const ACCOUNT_KEY = ['account', 'me'] as const;

export function useAccount() {
  const client = useApiClient();

  return useQuery<Account, ApiError>({
    queryKey: ACCOUNT_KEY,
    queryFn: async () => {
      const { data } = await client.get<Account>('accounts/me');
      return data;
    },
    staleTime: Infinity,
  });
}

export function useUpdateAccount() {
  const client = useApiClient();
  const queryClient = useQueryClient();

  return useMutation<Account, ApiError, UpdateRequest>({
    mutationFn: async (request) => {
      const { data } = await client.put<Account>('accounts/me', request);
      return data;
    },
    onSuccess: (data) => {
      queryClient.setQueryData<Account>(ACCOUNT_KEY, data);
    },
  });
}

export function useAddEmail() {
  const client = useApiClient();
  const queryClient = useQueryClient();

  return useMutation<Email, ApiError, EmailRequest>({
    mutationFn: async (request) => {
      const { data } = await client.post<Email>('accounts/me/emails', request);
      return data;
    },
    onSuccess: (email) => {
      queryClient.setQueryData<Account>(ACCOUNT_KEY, (prev) => {
        if (!prev) return prev;
        return { ...prev, emails: [...prev.emails, email] };
      });
    },
  });
}

export function useDeleteEmail() {
  const client = useApiClient();
  const queryClient = useQueryClient();

  return useMutation<void, ApiError, string>({
    mutationFn: async (email) => {
      await client.delete(`accounts/me/emails/${email}`);
    },
    onSuccess: (_, email) => {
      queryClient.setQueryData<Account>(ACCOUNT_KEY, (prev) => {
        if (!prev) return prev;
        return { ...prev, emails: prev.emails.filter((e) => e.value !== email) };
      });
    },
  });
}

export function useSetPrimaryEmail() {
  const client = useApiClient();
  const queryClient = useQueryClient();

  return useMutation<Email[], ApiError, EmailRequest>({
    mutationFn: async (request) => {
      const { data } = await client.post<Email[]>('accounts/me/emails/status', request);
      return data;
    },
    onSuccess: (emails) => {
      queryClient.setQueryData<Account>(ACCOUNT_KEY, (prev) => {
        if (!prev) return prev;
        return { ...prev, emails };
      });
    },
  });
}

export function useRequestEmailVerification() {
  const client = useApiClient();

  return useMutation<Confirming, ApiError, EmailRequest>({
    mutationFn: async (request) => {
      const { data } = await client.post<Confirming>('accounts/me/emails/verify', request);
      return data;
    },
  });
}

export function useConfirmEmail() {
  const client = useApiClient();
  const queryClient = useQueryClient();

  return useMutation<Email, ApiError, VerifyRequest>({
    mutationFn: async (request) => {
      const { data } = await client.put<Email>('accounts/me/emails/verify', request);
      return data;
    },
    onSuccess: (email) => {
      queryClient.setQueryData<Account>(ACCOUNT_KEY, (prev) => {
        if (!prev) return prev;
        return {
          ...prev,
          emails: prev.emails.map((e) => (e.value === email.value ? email : e)),
        };
      });
    },
  });
}

export function useDeleteConnection() {
  const client = useApiClient();
  const queryClient = useQueryClient();

  return useMutation<
    void,
    ApiError,
    { service: string; identity: string }
  >({
    mutationFn: async ({ service, identity }) => {
      await client.delete(
        `accounts/me/connections/${service}/identities/${identity}`
      );
    },
    onSuccess: (_, { service, identity }) => {
      queryClient.setQueryData<Account>(ACCOUNT_KEY, (prev) => {
        if (!prev) return prev;
        return {
          ...prev,
          connections: prev.connections.filter(
            (c) => !(c.type === service && c.identity === identity)
          ),
        };
      });
    },
  });
}
