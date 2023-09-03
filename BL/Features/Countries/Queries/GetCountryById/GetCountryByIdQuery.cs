using BL.DTOs;
using MediatR;

namespace BL.Features.Countries.Queries.GetCountryById
{
    public record GetCountryByIdQuery(int id, bool isTracked = true) : IRequest<CountryDto?>;
}
