using AutoMapper;
using ECommerce.Api.Products.Data;
using ECommerce.Api.Products.Dtos;
using ECommerce.Api.Products.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Providers
{
    public class ProductsProvider : IProductsProvider
    {
        private readonly ProductsDbContext dbContext;
        private readonly ILogger<ProductsProvider> logger;
        private readonly IMapper mapper;

        public ProductsProvider(ProductsDbContext context, ILogger<ProductsProvider> logger, IMapper mapper)
        {
            this.dbContext = context;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Products.Any())
            {
                List<Product> products = new List<Product>
                {
                    new Product() {Id = 1,Name = "Mouse" , Description = "Laptop Accessories",Price = 5,Inventory=100},
                    new Product() {Id = 2,Name = "Keyboard" , Description = "Touchable keyboard",Price = 20,Inventory=200},
                    new Product() {Id = 3,Name = "Screen" , Description = "Touchable Screen",Price = 150,Inventory=200},
                    new Product() {Id = 4,Name = "Laptop" , Description = "Laptop with Size 20 * 20",Price = 500,Inventory=300},
                    new Product() {Id = 5,Name = "Speaker" , Description = "Laptop Accessories",Price =20,Inventory=100},
                };
                dbContext.Products.AddRange(products);
                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<ProductDto> products, string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                logger?.LogInformation("Querying Products");
                var products = await dbContext.Products.ToListAsync();
                if(products != null && products.Any())
                {
                    logger?.LogInformation($"{products.Count} products found");
                    var result = mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
                    return (true, result, null);
                }
                return (false, null, "No data found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, ProductDto product, string ErrorMessage)> GetProductAsync(int id)
        {
            try
            {
                var product = await dbContext.Products.FirstOrDefaultAsync(a => a.Id == id);
                if (product != null)
                {
                    var result = mapper.Map<Product, ProductDto>(product);
                    return (true, result, null);
                }

                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
