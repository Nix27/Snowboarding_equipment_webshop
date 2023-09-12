using AutoMapper;
using DAL.Models;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Countries.Commands.UpdateCountry
{
    internal class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCountryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
        {
            var categoryForUpdate = _mapper.Map<Country>(request.countryForUpdate);

            _unitOfWork.Country.Update(categoryForUpdate);
            await _unitOfWork.SaveAsync();

            return categoryForUpdate.Id;
        }
    }
}
