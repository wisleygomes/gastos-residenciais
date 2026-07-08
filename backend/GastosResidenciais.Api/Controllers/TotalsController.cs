using GastosResidenciais.Api.DTOs;
using GastosResidenciais.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GastosResidenciais.Api.Controllers;

/// <summary>
/// Endpoint de consulta de totais: totais por pessoa (receitas, despesas, saldo)
/// e o total geral consolidado de todas as pessoas.
/// </summary>
[ApiController]
[Route("api/totals")]
public class TotalsController : ControllerBase
{
    private readonly ITotalsService _totalsService;

    public TotalsController(ITotalsService totalsService)
    {
        _totalsService = totalsService;
    }

    /// <summary>GET /api/totals - retorna o relatório consolidado de totais.</summary>
    [HttpGet]
    public async Task<ActionResult<TotalsReportDto>> Get()
    {
        var report = await _totalsService.GetReportAsync();
        return Ok(report);
    }
}
