namespace GastosResidenciais.Api.Models;

/// <summary>
/// Representa o tipo de uma transação financeira.
/// Despesa (Expense) reduz o saldo da pessoa, Receita (Income) aumenta o saldo.
/// </summary>
public enum TransactionType
{
    Expense = 0, // Despesa
    Income = 1   // Receita
}
