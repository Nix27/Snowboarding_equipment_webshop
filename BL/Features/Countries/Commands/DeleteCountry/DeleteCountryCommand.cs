using BL.DTOs;
using MediatR;

namespace BL.Features.Countries.Commands.DeleteCountry
{
    public record DeleteCountryCommand(int countryId) : IRequest<int>;
}
