using AutoMapper;
using ECommerce.Api.Products.Data;
using ECommerce.Api.Products.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Profiles
{
    public class ProductProfiler : Profile
    {
        public ProductProfiler()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
