using AutoMapper;
using DAL.Models;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Countries.Commands.DeleteCountry
{
    internal class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteCountryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
        {
            var countryForDelete = _mapper.Map<Country>(request.countryForDelete);

            _unitOfWork.Country.Delete(countryForDelete);
            await _unitOfWork.SaveAsync();

            return countryForDelete.Id;
        }
    }
}
