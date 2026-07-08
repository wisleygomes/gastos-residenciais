using GastosResidenciais.Api.Data;
using GastosResidenciais.Api.DTOs;
using GastosResidenciais.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GastosResidenciais.Api.Services;

/// <summary>
/// Responsável por consolidar os totais de receitas, despesas e saldo
/// por pessoa e o total geral de todas as pessoas cadastradas.
/// </summary>
public class TotalsService : ITotalsService
{
    private readonly AppDbContext _db;

    public TotalsService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<TotalsReportDto> GetReportAsync()
    {
        // Carrega todas as pessoas junto com suas transações em uma única consulta
        // (evita N+1 queries ao calcular os totais de cada uma).
        var people = await _db.People
            .Include(p => p.Transactions)
            .OrderBy(p => p.Name)
            .ToListAsync();

        var peopleTotals = new List<PersonTotalsDto>();

        decimal grandIncome = 0m;
        decimal grandExpense = 0m;

        foreach (var person in people)
        {
            var income = person.Transactions
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Value);

            var expense = person.Transactions
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Value);

            var balance = income - expense;

            peopleTotals.Add(new PersonTotalsDto(person.Id, person.Name, income, expense, balance));

            grandIncome += income;
            grandExpense += expense;
        }

        return new TotalsReportDto(
            peopleTotals,
            grandIncome,
            grandExpense,
            grandIncome - grandExpense
        );
    }
}
