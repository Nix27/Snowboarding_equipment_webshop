using AutoMapper;
using BL.DTOs;
using BL.Features.Countries.Commands.CreateCountry;
using DAL.Models;
using DAL.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.Countries.Commands.DeleteCountry
{
    public class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, CountryDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteCountryCommand> _logger;

        public DeleteCountryCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<DeleteCountryCommand> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CountryDto?> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var countryForDelete = _mapper.Map<Country>(request.countryForDelete);

                _unitOfWork.Country.Delete(countryForDelete);
                await _unitOfWork.SaveAsync();

                return _mapper.Map<CountryDto>(countryForDelete);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return null;
            }
        }
    }
}
