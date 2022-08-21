﻿using AutoMapper;
using EShop.Api.Models.RequestModels;
using EShop.Api.Models.ViewModels;
using EShop.Domain.Common;
using EShop.Domain.Models;

namespace EShop.Api.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<ProductUpdateRequest, Product>();
            CreateMap<Product, ProductCreateRequest>();
            CreateMap<Product, ProductViewModel>();
            CreateMap<Order, OrderCompactViewModel>();
        }
    }
}
