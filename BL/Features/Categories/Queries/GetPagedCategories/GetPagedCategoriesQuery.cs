using BL.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.Categories.Queries.GetPagedCategories
{
    public record GetPagedCategoriesQuery(int page, int size, string? searchTerm) : IRequest<IEnumerable<CategoryDto>?>;
}
