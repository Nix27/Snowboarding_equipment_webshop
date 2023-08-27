using AutoMapper;
using BL.DTOs;
using DAL.Models;
using DAL.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, CategoryDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteCategoryCommand> _logger;

        public DeleteCategoryCommandHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            ILogger<DeleteCategoryCommand> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CategoryDto?> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var categoryForDelete = _mapper.Map<Category>(request.categoryForDelete);

                _unitOfWork.Category.Delete(categoryForDelete);
                await _unitOfWork.SaveAsync();

                return _mapper.Map<CategoryDto>(categoryForDelete);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return null;
            }
        }
    }
}
