using BL.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.Categories.Commands.CreateCategory
{
    public record CreateCategoryCommand(CategoryDto newCategory) : IRequest<CategoryDto?>;
}
