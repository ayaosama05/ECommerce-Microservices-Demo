using ECommerce.Api.Products.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Interfaces
{
    public interface IProductsProvider
    {
        Task<(bool IsSuccess, IEnumerable<ProductDto> products, string ErrorMessage)> GetProductsAsync();
        Task<(bool IsSuccess, ProductDto product, string ErrorMessage)> GetProductAsync(int id);

    }
}
