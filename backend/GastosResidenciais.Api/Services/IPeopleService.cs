using GastosResidenciais.Api.DTOs;

namespace GastosResidenciais.Api.Services;

public interface IPeopleService
{
    Task<List<PersonDto>> ListAsync();
    Task<PersonDto> CreateAsync(CreatePersonDto dto);
    Task DeleteAsync(Guid id);
}
