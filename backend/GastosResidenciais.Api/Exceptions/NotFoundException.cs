namespace GastosResidenciais.Api.Exceptions;

/// <summary>
/// Lançada quando um recurso solicitado (pessoa, transação, etc.) não existe.
/// Traduzida pelo middleware global em uma resposta HTTP 404.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}
