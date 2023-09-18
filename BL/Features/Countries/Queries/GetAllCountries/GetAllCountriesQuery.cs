using BL.DTOs;
using DAL.Models;
using MediatR;
using System.Linq.Expressions;

namespace BL.Features.Countries.Queries.GetAllCountries
{
    public record GetAllCountriesQuery(
        Expression<Func<Country, bool>>? filter = null,
        string? includeProperties = null, 
        bool isTracked = true) : IRequest<IEnumerable<CountryDto>>;
}
