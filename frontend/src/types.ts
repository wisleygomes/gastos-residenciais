// Tipos que espelham os DTOs expostos pela API .NET (ver backend/GastosResidenciais.Api/DTOs).

/** Tipo de transação: precisa bater com o enum TransactionType do backend (0 = Despesa, 1 = Receita). */
export enum TransactionType {
  Expense = 0, // Despesa
  Income = 1, // Receita
}

export interface Person {
  id: string;
  name: string;
  age: number;
  isMinor: boolean;
}

export interface CreatePersonInput {
  name: string;
  age: number;
}

export interface Transaction {
  id: string;
  description: string;
  value: number;
  type: TransactionType;
  personId: string;
  personName: string;
  createdAt: string;
}

export interface CreateTransactionInput {
  description: string;
  value: number;
  type: TransactionType;
  personId: string;
}

export interface PersonTotals {
  personId: string;
  personName: string;
  totalIncome: number;
  totalExpense: number;
  balance: number;
}

export interface TotalsReport {
  people: PersonTotals[];
  grandTotalIncome: number;
  grandTotalExpense: number;
  grandTotalBalance: number;
}

/** Formato padronizado de erro retornado pelo middleware global da API. */
export interface ApiErrorPayload {
  message: string;
}
