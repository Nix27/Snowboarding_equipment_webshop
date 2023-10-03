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

        public Task<IEnumerable<CategoryDto>> Handle(GetPagedCategoriesQuery request, CancellationToken cancellationToken)
        { 
            var pagedCategories = request.categories.Skip((request.page - 1) * (int)request.size).Take((int)request.size);

            return Task.FromResult(pagedCategories);
        }
    }
}
