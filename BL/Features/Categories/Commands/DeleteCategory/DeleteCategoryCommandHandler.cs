using AutoMapper;
using DAL.Models;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Categories.Commands.DeleteCategory
{
    internal class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryForDelete = _mapper.Map<Category>(request.categoryForDelete);

            _unitOfWork.Category.Delete(categoryForDelete);
            await _unitOfWork.SaveAsync();

            return categoryForDelete.Id;
        }
    }
}
