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

namespace BL.Features.Categories.Queries.GetAllCategories
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllCategoriesQuery> _logger;

        public GetAllCategoriesQueryHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            ILogger<GetAllCategoriesQuery> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CategoryDto>?> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allCategories = await _unitOfWork.Category.GetAllAsync();
                return _mapper.Map<IEnumerable<CategoryDto>>(allCategories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return null;
            }
        }
    }
}
