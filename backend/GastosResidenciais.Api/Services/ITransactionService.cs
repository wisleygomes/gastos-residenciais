using GastosResidenciais.Api.DTOs;

namespace GastosResidenciais.Api.Services;

public interface ITransactionService
{
    Task<List<TransactionDto>> ListAsync();
    Task<TransactionDto> CreateAsync(CreateTransactionDto dto);
}
