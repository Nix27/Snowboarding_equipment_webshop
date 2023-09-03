using BL.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.Countries.Queries.GetAllCountries
{
    public record GetAllCountriesQuery() : IRequest<IEnumerable<CountryDto>>;
}
