using BL.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.Countries.Commands.CreateCountry
{
    public record CreateCountryCommand(CountryDto newCountry) : IRequest<int?>;
}
