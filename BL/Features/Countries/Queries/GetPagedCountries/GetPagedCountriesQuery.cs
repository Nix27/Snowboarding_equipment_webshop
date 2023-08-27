using BL.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.Countries.Queries.GetPagedCountries
{
    public record GetPagedCountriesQuery(int page, int size, string? searchTerm) : IRequest<IEnumerable<CountryDto>?>;
}
