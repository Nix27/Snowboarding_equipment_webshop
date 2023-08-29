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

namespace BL.Features.Countries.Commands.UpdateCountry
{
    public class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand, int?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCountryCommand> _logger;

        public UpdateCountryCommandHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            ILogger<UpdateCountryCommand> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int?> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var categoryForUpdate = _mapper.Map<Country>(request.countryForUpdate);

                _unitOfWork.Country.Update(categoryForUpdate);
                await _unitOfWork.SaveAsync();

                return categoryForUpdate.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return null;
            }
        }
    }
}
