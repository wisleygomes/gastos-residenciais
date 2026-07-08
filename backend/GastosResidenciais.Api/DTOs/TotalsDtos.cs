namespace GastosResidenciais.Api.DTOs;

/// <summary>
/// Totais consolidados (receitas, despesas e saldo) de uma pessoa específica.
/// </summary>
public record PersonTotalsDto(
    Guid PersonId,
    string PersonName,
    decimal TotalIncome,
    decimal TotalExpense,
    decimal Balance
);

/// <summary>
/// Resposta completa da consulta de totais: os totais por pessoa e o
/// total geral consolidado de todas as pessoas cadastradas.
/// </summary>
public record TotalsReportDto(
    List<PersonTotalsDto> People,
    decimal GrandTotalIncome,
    decimal GrandTotalExpense,
    decimal GrandTotalBalance
);
