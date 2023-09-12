using AutoMapper;
using BL.DTOs;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Categories.Queries.GetPagedCategories
{
    internal class GetPagedCategoriesQueryHandler : IRequestHandler<GetPagedCategoriesQuery, IEnumerable<CategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPagedCategoriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> Handle(GetPagedCategoriesQuery request, CancellationToken cancellationToken)
        {
            var allCategories = await _unitOfWork.Category.GetAllAsync();

            string? searchTerm = request.searchTerm?.ToLower();

            if (searchTerm != null)
            {
                allCategories = allCategories.Where(c => c.Name.ToLower().Contains(searchTerm));
            }

            var pagedCategories = allCategories.Skip(request.page * request.size).Take(request.size);

            return _mapper.Map<IEnumerable<CategoryDto>>(pagedCategories);
        }
    }
}
