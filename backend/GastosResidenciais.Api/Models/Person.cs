namespace GastosResidenciais.Api.Models;

/// <summary>
/// Entidade que representa uma pessoa cadastrada no sistema de controle de gastos residenciais.
/// </summary>
public class Person
{
    /// <summary>
    /// Identificador único, gerado automaticamente pelo sistema (Guid) no momento da criação.
    /// </summary>
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    /// <summary>
    /// Indica se a pessoa é menor de idade (menor de 18 anos).
    /// Usado pela regra de negócio que restringe menores a cadastrar apenas despesas.
    /// </summary>
    public bool IsMinor => Age < 18;

    /// <summary>
    /// Transações associadas a essa pessoa.
    /// Configurado no DbContext com delete em cascata: ao remover a pessoa,
    /// todas as suas transações são removidas automaticamente.
    /// </summary>
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
