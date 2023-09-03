using AutoMapper;
using DAL.Models;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryForUpdate = _mapper.Map<Category>(request.categoryForUpdate);

            _unitOfWork.Category.Update(categoryForUpdate);
            await _unitOfWork.SaveAsync();

            return categoryForUpdate.Id;
        }
    }
}
