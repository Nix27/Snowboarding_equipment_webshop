using BL.DTOs;
using MediatR;

namespace BL.Features.Countries.Queries.GetPagedCountries
{
    public record GetPagedCountriesQuery(IEnumerable<CountryDto>? countries, int page, int size) : IRequest<IEnumerable<CountryDto>>;
}
