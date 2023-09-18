using DAL.Models;
using MediatR;
using System.Linq.Expressions;

namespace BL.Features.Countries.Queries.GetNumberOfCountries
{
    public record GetNumberOfCountriesQuery(Expression<Func<Country, bool>>? filter = null) : IRequest<int>;
}
