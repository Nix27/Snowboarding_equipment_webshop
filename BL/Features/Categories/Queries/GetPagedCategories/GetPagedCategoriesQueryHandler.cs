using AutoMapper;
using BL.DTOs;
using BL.Features.Categories.Queries.GetAllCategories;
using MediatR;

namespace BL.Features.Categories.Queries.GetPagedCategories
{
    internal class GetPagedCategoriesQueryHandler : IRequestHandler<GetPagedCategoriesQuery, IEnumerable<CategoryDto>>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GetPagedCategoriesQueryHandler(IMediator mediator, IMapper mapper)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<IEnumerable<CategoryDto>> Handle(GetPagedCategoriesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<CategoryDto>? categories = request.categories;

            if(categories == null)
            {
                var categoriesFromDb = await _mediator.Send(new GetAllCategoriesQuery());
                categories = _mapper.Map<IEnumerable<CategoryDto>>(categoriesFromDb);
            }
                
            var pagedCategories = categories.Skip(request.page * request.size).Take(request.size);

            return pagedCategories;
        }
    }
}
