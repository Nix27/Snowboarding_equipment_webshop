using BL.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.Countries.Commands.UpdateCountry
{
    public record UpdateCountryCommand(CountryDto countryForUpdate) : IRequest<int>;
}
