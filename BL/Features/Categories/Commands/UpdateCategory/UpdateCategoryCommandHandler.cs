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

namespace BL.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCategoryCommand> _logger;

        public UpdateCategoryCommandHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            ILogger<UpdateCategoryCommand> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CategoryDto?> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var categoryForUpdate = _mapper.Map<Category>(request.categoryForUpdate);

                _unitOfWork.Category.Update(categoryForUpdate);
                await _unitOfWork.SaveAsync();

                return _mapper.Map<CategoryDto>(categoryForUpdate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return null;
            }
        }
    }
}
