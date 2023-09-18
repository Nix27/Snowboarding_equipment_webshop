using AutoMapper;
using BL.Features.Countries.Queries.GetCountryById;
using DAL.Models;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Countries.Commands.DeleteCountry
{
    internal class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICountryRepository _countryRepository;
        private readonly IMediator _mediator;

        public DeleteCountryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICountryRepository countryRepository, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _countryRepository = countryRepository;
            _mediator = mediator;
        }

        public async Task<int> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
        {
            var countryForDeleteDto = await _mediator.Send(new GetCountryByIdQuery(request.countryId, isTracked: false));

            var countryForDelete = _mapper.Map<Country>(countryForDeleteDto);

            _countryRepository.Delete(countryForDelete);
            await _unitOfWork.SaveAsync();

            return countryForDeleteDto.Id;
        }
    }
}
