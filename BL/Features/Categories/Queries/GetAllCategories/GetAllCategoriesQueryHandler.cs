using AutoMapper;
using BL.DTOs;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Categories.Queries.GetAllCategories
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var allCategories = await _unitOfWork.Category.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(allCategories);
        }
    }
}
