using GastosResidenciais.Api.DTOs;
using GastosResidenciais.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GastosResidenciais.Api.Controllers;

/// <summary>
/// Endpoints REST para o cadastro de transações: criação e listagem.
/// (Edição e deleção não fazem parte do escopo do desafio.)
/// </summary>
[ApiController]
[Route("api/transactions")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    /// <summary>GET /api/transactions - lista todas as transações cadastradas.</summary>
    [HttpGet]
    public async Task<ActionResult<List<TransactionDto>>> List()
    {
        var transactions = await _transactionService.ListAsync();
        return Ok(transactions);
    }

    /// <summary>
    /// POST /api/transactions - cadastra uma nova transação.
    /// Aplica a regra de negócio: se a pessoa for menor de idade, apenas
    /// despesas são aceitas (validado na camada de serviço).
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TransactionDto>> Create([FromBody] CreateTransactionDto dto)
    {
        var created = await _transactionService.CreateAsync(dto);
        return CreatedAtAction(nameof(List), new { id = created.Id }, created);
    }
}
