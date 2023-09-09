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
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<CategoryVM, CategoryDto>().ReverseMap();
            CreateMap<CountryDto, Country>().ReverseMap();
            CreateMap<CountryVM, CountryDto>().ReverseMap();
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<ProductVM, ProductDto>().ReverseMap();
            CreateMap<ThumbnailImageDto, ThumbnailImage>().ReverseMap();    
            CreateMap<GalleryImageDto, GalleryImage>().ReverseMap();
            CreateMap<PageProductsRequestVM, PageProductsRequestDto>().ReverseMap();
            CreateMap<FilterCustomerProductsRequestVM, FilterCustomerProductsRequestDto>().ReverseMap();
        }
    }
}
