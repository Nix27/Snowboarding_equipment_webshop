using AutoMapper;
using BL.DTOs;
using DAL.Models;
using DAL.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.Countries.Commands.CreateCountry
{
    public class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, int?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCountryCommand> _logger;

        public CreateCountryCommandHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            ILogger<CreateCountryCommand> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int?> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newCountry = _mapper.Map<Country>(request.newCountry);

                await _unitOfWork.Country.CreateAsync(newCountry);
                await _unitOfWork.SaveAsync();

                return newCountry.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return null;
            }
        }
    }
}
