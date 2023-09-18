using AutoMapper;
using BL.Features.Categories.Queries.GetCategoryById;
using DAL.Models;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Categories.Commands.DeleteCategory
{
    internal class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork, IMediator mediator, IMapper mapper, ICategoryRepository categoryRepository)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<int> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryForDeleteDto = await _mediator.Send(new GetCategoryByIdQuery(request.categoryId, isTracked: false));

            var categoryForDelete = _mapper.Map<Category>(categoryForDeleteDto);

            _categoryRepository.Delete(categoryForDelete);
            await _unitOfWork.SaveAsync();

            return categoryForDeleteDto.Id;
        }
    }
}
