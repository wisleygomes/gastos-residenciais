using GastosResidenciais.Api.Data;
using GastosResidenciais.Api.DTOs;
using GastosResidenciais.Api.Exceptions;
using GastosResidenciais.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GastosResidenciais.Api.Services;

/// <summary>
/// Regras de negócio referentes ao cadastro de transações (receitas/despesas).
/// </summary>
public class TransactionService : ITransactionService
{
    private readonly AppDbContext _db;

    public TransactionService(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Lista todas as transações cadastradas, das mais recentes para as mais antigas,
    /// já trazendo o nome da pessoa associada.
    /// </summary>
    public async Task<List<TransactionDto>> ListAsync()
    {
        return await _db.Transactions
            .Include(t => t.Person)
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TransactionDto(
                t.Id,
                t.Description,
                t.Value,
                t.Type,
                t.PersonId,
                t.Person!.Name,
                t.CreatedAt))
            .ToListAsync();
    }

    /// <summary>
    /// Cria uma nova transação, validando:
    /// 1) que a pessoa informada (PersonId) existe no cadastro;
    /// 2) que o valor é positivo;
    /// 3) a regra de negócio de menores de idade: pessoas com menos de 18 anos
    ///    só podem ter despesas cadastradas, nunca receitas.
    /// </summary>
    public async Task<TransactionDto> CreateAsync(CreateTransactionDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Description))
        {
            throw new BusinessRuleException("A descrição da transação é obrigatória.");
        }

        if (dto.Value <= 0)
        {
            throw new BusinessRuleException("O valor da transação deve ser maior que zero.");
        }

        var person = await _db.People.FindAsync(dto.PersonId);
        if (person is null)
        {
            throw new NotFoundException($"Pessoa com Id '{dto.PersonId}' não encontrada. " +
                "A transação deve estar vinculada a uma pessoa já cadastrada.");
        }

        // Regra de negócio central: menor de idade (idade < 18) só pode registrar despesas.
        if (person.IsMinor && dto.Type == TransactionType.Income)
        {
            throw new BusinessRuleException(
                $"'{person.Name}' é menor de idade ({person.Age} anos) e, portanto, " +
                "só pode ter despesas cadastradas, não receitas.");
        }

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Description = dto.Description.Trim(),
            Value = dto.Value,
            Type = dto.Type,
            PersonId = person.Id,
            CreatedAt = DateTime.UtcNow
        };

        _db.Transactions.Add(transaction);
        await _db.SaveChangesAsync();

        return new TransactionDto(
            transaction.Id,
            transaction.Description,
            transaction.Value,
            transaction.Type,
            transaction.PersonId,
            person.Name,
            transaction.CreatedAt);
    }
}
