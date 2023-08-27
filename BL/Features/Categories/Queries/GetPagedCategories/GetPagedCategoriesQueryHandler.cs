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

namespace BL.Features.Categories.Queries.GetPagedCategories
{
    public class GetPagedCategoriesQueryHandler : IRequestHandler<GetPagedCategoriesQuery, IEnumerable<CategoryDto>?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPagedCategoriesQuery> _logger;

        public GetPagedCategoriesQueryHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            ILogger<GetPagedCategoriesQuery> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CategoryDto>?> Handle(GetPagedCategoriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allCategories = await _unitOfWork.Category.GetAllAsync();

                string? searchTerm = request.searchTerm?.ToLower();

                if(searchTerm != null)
                {
                    allCategories = allCategories.Where(c => c.Name.ToLower().Contains(searchTerm));
                }

                var pagedCategories = allCategories.Skip(request.page * request.size).Take(request.size);

                return _mapper.Map<IEnumerable<CategoryDto>>(pagedCategories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return null;
            }
        }
    }
}
