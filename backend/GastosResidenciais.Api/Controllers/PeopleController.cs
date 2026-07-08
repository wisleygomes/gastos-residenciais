using GastosResidenciais.Api.DTOs;
using GastosResidenciais.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GastosResidenciais.Api.Controllers;

/// <summary>
/// Endpoints REST para o cadastro de pessoas: criação, listagem e deleção.
/// </summary>
[ApiController]
[Route("api/people")]
public class PeopleController : ControllerBase
{
    private readonly IPeopleService _peopleService;

    public PeopleController(IPeopleService peopleService)
    {
        _peopleService = peopleService;
    }

    /// <summary>GET /api/people - lista todas as pessoas cadastradas.</summary>
    [HttpGet]
    public async Task<ActionResult<List<PersonDto>>> List()
    {
        var people = await _peopleService.ListAsync();
        return Ok(people);
    }

    /// <summary>POST /api/people - cadastra uma nova pessoa.</summary>
    [HttpPost]
    public async Task<ActionResult<PersonDto>> Create([FromBody] CreatePersonDto dto)
    {
        var created = await _peopleService.CreateAsync(dto);
        return CreatedAtAction(nameof(List), new { id = created.Id }, created);
    }

    /// <summary>
    /// DELETE /api/people/{id} - remove uma pessoa e, em cascata,
    /// todas as transações associadas a ela.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _peopleService.DeleteAsync(id);
        return NoContent();
    }
}
