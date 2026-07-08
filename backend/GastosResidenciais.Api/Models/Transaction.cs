namespace GastosResidenciais.Api.Models;

/// <summary>
/// Entidade que representa uma transação financeira (receita ou despesa)
/// vinculada a uma pessoa cadastrada.
/// </summary>
public class Transaction
{
    /// <summary>
    /// Identificador único, gerado automaticamente pelo sistema (Guid) no momento da criação.
    /// </summary>
    public Guid Id { get; set; }

    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Valor monetário da transação. Sempre armazenado como valor positivo;
    /// o sinal (soma/subtração) é determinado pelo campo Type na hora de calcular totais.
    /// </summary>
    public decimal Value { get; set; }

    public TransactionType Type { get; set; }

    /// <summary>
    /// Chave estrangeira para a pessoa dona da transação. Deve referenciar
    /// um Id existente no cadastro de pessoas (validado na camada de serviço).
    /// </summary>
    public Guid PersonId { get; set; }

    public Person? Person { get; set; }

    /// <summary>
    /// Data/hora de criação da transação, útil para auditoria e ordenação na listagem.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
