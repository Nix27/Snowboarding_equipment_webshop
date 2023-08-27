using BL.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.Countries.Commands.DeleteCountry
{
    public record DeleteCountryCommand(CountryDto countryForDelete) : IRequest<CountryDto?>;
}
