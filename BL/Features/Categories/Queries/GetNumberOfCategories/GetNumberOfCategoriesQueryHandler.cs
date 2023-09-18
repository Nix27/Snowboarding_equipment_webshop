using BL.Features.Categories.Queries.GetAllCategories;
using MediatR;

namespace BL.Features.Categories.Queries.GetNumberOfCategories
{
    internal class GetNumberOfCategoriesQueryHandler : IRequestHandler<GetNumberOfCategoriesQuery, int>
    {
        private readonly IMediator _mediator;

        public GetNumberOfCategoriesQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<int> Handle(GetNumberOfCategoriesQuery request, CancellationToken cancellationToken)
        {
            return _mediator.Send(new GetAllCategoriesQuery(request.filter, isTracked:false))
                            .GetAwaiter()
                            .GetResult()
                            .Count();
        }
    }
}
