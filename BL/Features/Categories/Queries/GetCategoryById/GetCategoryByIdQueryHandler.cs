using AutoMapper;
using BL.DTOs;
using DAL.Repositories.Interfaces;
using MediatR;

namespace BL.Features.Categories.Queries.GetCategoryById
{
    internal class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByIdQueryHandler(IMapper mapper, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var requestedCategory = await _categoryRepository.GetFirstOrDefaultAsync(c => c.Id == request.id, isTracked: request.isTracked);

            if (requestedCategory == null)
                throw new InvalidOperationException("Category not found");
            
            return _mapper.Map<CategoryDto>(requestedCategory);
        }
    }
}
