using AutoMapper;
using EShop.Api.Models.ViewModels;
using EShop.Domain.Models;

namespace EShop.Api.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Product, ProductUpdateRequest>().ReverseMap();
        }
    }
}
