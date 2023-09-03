using AutoMapper;
using DAL.Models;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Countries.Commands.CreateCountry
{
    public class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateCountryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            var newCountry = _mapper.Map<Country>(request.newCountry);

            await _unitOfWork.Country.CreateAsync(newCountry);
            await _unitOfWork.SaveAsync();

            return newCountry.Id;
        }
    }
}
