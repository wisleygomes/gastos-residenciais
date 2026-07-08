namespace GastosResidenciais.Api.Exceptions;

/// <summary>
/// Lançada quando uma regra de negócio é violada (ex: menor de idade tentando
/// cadastrar uma receita, valor inválido, etc).
/// Traduzida pelo middleware global em uma resposta HTTP 400.
/// </summary>
public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message) { }
}
