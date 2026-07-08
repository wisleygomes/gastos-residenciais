using GastosResidenciais.Api.DTOs;

namespace GastosResidenciais.Api.Services;

public interface ITotalsService
{
    Task<TotalsReportDto> GetReportAsync();
}
