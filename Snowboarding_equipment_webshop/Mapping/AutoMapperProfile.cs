using AutoMapper;
using BL.DTOs;
using DAL.Models;
using Snowboarding_equipment_webshop.ViewModels;

namespace Snowboarding_equipment_webshop.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CategoryVM, CategoryDto>().ReverseMap();
        }
    }
}
