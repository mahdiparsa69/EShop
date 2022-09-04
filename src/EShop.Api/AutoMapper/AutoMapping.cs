using AutoMapper;
using EShop.Api.Models.RequestModels;
using EShop.Api.Models.RequstModels;
using EShop.Api.Models.ViewModels;
using EShop.Domain.Models;

namespace EShop.Api.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<ProductUpdateRequest, Product>();

            CreateMap<Product, ProductCreateRequest>().ReverseMap();

            CreateMap<Product, ProductViewModel>();

            CreateMap<Order, OrderCompactViewModel>();

            CreateMap<OrderCreateRequest, Order>();

            CreateMap<OrderUpdateRequest, Order>();

            CreateMap<UserRequestModel, User>();

            CreateMap<UserCreateModel, User>();

            CreateMap<User, UserViewModel>();
        }
    }
}
