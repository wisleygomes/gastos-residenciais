using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.DTOs;

/// <summary>
/// Dados necessários para criar uma nova transação.
/// </summary>
public record CreateTransactionDto(string Description, decimal Value, TransactionType Type, Guid PersonId);

/// <summary>
/// Representação de uma transação retornada pela API, já incluindo
/// o nome da pessoa para facilitar a exibição no front-end sem uma
/// chamada adicional.
/// </summary>
public record TransactionDto(
    Guid Id,
    string Description,
    decimal Value,
    TransactionType Type,
    Guid PersonId,
    string PersonName,
    DateTime CreatedAt
);
