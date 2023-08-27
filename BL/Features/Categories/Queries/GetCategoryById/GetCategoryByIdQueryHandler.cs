using AutoMapper;
using BL.DTOs;
using DAL.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCategoryByIdQuery> _logger;

        public GetCategoryByIdQueryHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            ILogger<GetCategoryByIdQuery> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var requestedCategory = await _unitOfWork.Category.GetFirstOrDefaultAsync(c => c.Id == request.id, isTracked: request.isTracked);
                return _mapper.Map<CategoryDto>(requestedCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return null;
            }
        }
    }
}
