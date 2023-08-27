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

namespace BL.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCategoryCommand> _logger;

        public CreateCategoryCommandHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ILogger<CreateCategoryCommand> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CategoryDto?> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newCategory = _mapper.Map<Category>(request.newCategory);

                await _unitOfWork.Category.CreateAsync(newCategory);
                await _unitOfWork.SaveAsync();

                return _mapper.Map<CategoryDto>(newCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return null;
            }
        }
    }
}
