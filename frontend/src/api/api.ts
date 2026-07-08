import type {
  Person,
  CreatePersonInput,
  Transaction,
  CreateTransactionInput,
  TotalsReport,
  ApiErrorPayload,
} from '../types';

// URL base da API .NET. Em produção, isso pode ser movido para uma variável de ambiente (import.meta.env).
const API_BASE_URL = 'http://localhost:5000/api';

/**
 * Erro customizado que carrega a mensagem amigável enviada pelo backend
 * (extraída do corpo { message: "..." } retornado pelo ExceptionHandlingMiddleware).
 */
export class ApiError extends Error {}

/**
 * Wrapper genérico em torno do fetch: monta a URL, define os headers JSON
 * e converte respostas de erro no formato do backend em uma ApiError legível.
 */
async function request<T>(path: string, options?: RequestInit): Promise<T> {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    headers: { 'Content-Type': 'application/json' },
    ...options,
  });

  if (!response.ok) {
    let message = `Erro ${response.status} ao comunicar com o servidor.`;
    try {
      const errorBody = (await response.json()) as ApiErrorPayload;
      if (errorBody?.message) {
        message = errorBody.message;
      }
    } catch {
      // corpo da resposta não era JSON válido; mantém a mensagem genérica
    }
    throw new ApiError(message);
  }

  // 204 No Content (usado pelo DELETE de pessoas) não possui corpo para parsear.
  if (response.status === 204) {
    return undefined as T;
  }

  return (await response.json()) as T;
}

// ---------------------------------------------------------------------
// Pessoas
// ---------------------------------------------------------------------

export const peopleApi = {
  list: () => request<Person[]>('/people'),

  create: (input: CreatePersonInput) =>
    request<Person>('/people', {
      method: 'POST',
      body: JSON.stringify(input),
    }),

  remove: (id: string) =>
    request<void>(`/people/${id}`, {
      method: 'DELETE',
    }),
};

// ---------------------------------------------------------------------
// Transações
// ---------------------------------------------------------------------

export const transactionsApi = {
  list: () => request<Transaction[]>('/transactions'),

  create: (input: CreateTransactionInput) =>
    request<Transaction>('/transactions', {
      method: 'POST',
      body: JSON.stringify(input),
    }),
};

// ---------------------------------------------------------------------
// Totais
// ---------------------------------------------------------------------

export const totalsApi = {
  get: () => request<TotalsReport>('/totals'),
};
