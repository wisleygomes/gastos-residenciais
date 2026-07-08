namespace GastosResidenciais.Api.DTOs;

/// <summary>
/// Dados necessários para criar uma nova pessoa.
/// Não inclui o Id, pois este é gerado automaticamente pelo servidor.
/// </summary>
public record CreatePersonDto(string Name, int Age);

/// <summary>
/// Representação de uma pessoa retornada pela API.
/// </summary>
public record PersonDto(Guid Id, string Name, int Age, bool IsMinor);
