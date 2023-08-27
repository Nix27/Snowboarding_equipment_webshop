using BL.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.Categories.Queries.GetCategoryById
{
    public record GetCategoryByIdQuery(int id, bool isTracked = true) : IRequest<CategoryDto?>;
}
