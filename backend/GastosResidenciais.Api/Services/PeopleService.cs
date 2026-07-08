using GastosResidenciais.Api.Data;
using GastosResidenciais.Api.DTOs;
using GastosResidenciais.Api.Exceptions;
using GastosResidenciais.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GastosResidenciais.Api.Services;

/// <summary>
/// Regras de negócio referentes ao cadastro de pessoas.
/// </summary>
public class PeopleService : IPeopleService
{
    private readonly AppDbContext _db;

    public PeopleService(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Lista todas as pessoas cadastradas, ordenadas por nome.
    /// </summary>
    public async Task<List<PersonDto>> ListAsync()
    {
        return await _db.People
            .OrderBy(p => p.Name)
            .Select(p => new PersonDto(p.Id, p.Name, p.Age, p.Age < 18))
            .ToListAsync();
    }

    /// <summary>
    /// Cria uma nova pessoa. O Id é sempre gerado pelo servidor (Guid.NewGuid),
    /// nunca aceito do cliente, garantindo unicidade.
    /// </summary>
    public async Task<PersonDto> CreateAsync(CreatePersonDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new BusinessRuleException("O nome da pessoa é obrigatório.");
        }

        if (dto.Age < 0 || dto.Age > 150)
        {
            throw new BusinessRuleException("Idade inválida.");
        }

        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = dto.Name.Trim(),
            Age = dto.Age
        };

        _db.People.Add(person);
        await _db.SaveChangesAsync();

        return new PersonDto(person.Id, person.Name, person.Age, person.IsMinor);
    }

    /// <summary>
    /// Remove uma pessoa do cadastro. Como o relacionamento Person -> Transaction
    /// está configurado com DeleteBehavior.Cascade no AppDbContext, todas as
    /// transações dessa pessoa são apagadas automaticamente pelo banco de dados.
    /// </summary>
    public async Task DeleteAsync(Guid id)
    {
        var person = await _db.People.FindAsync(id);
        if (person is null)
        {
            throw new NotFoundException($"Pessoa com Id '{id}' não encontrada.");
        }

        _db.People.Remove(person);
        await _db.SaveChangesAsync();
    }
}
