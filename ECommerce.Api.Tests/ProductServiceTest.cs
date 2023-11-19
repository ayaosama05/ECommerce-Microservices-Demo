using AutoMapper;
using ECommerce.Api.Products.Data;
using ECommerce.Api.Products.Interfaces;
using ECommerce.Api.Products.Profiles;
using ECommerce.Api.Products.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Api.Tests
{
    public class ProductServiceTest
    {
        private ProductsDbContext ConfigureDatabase(string databaseName)
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            var context = new ProductsDbContext(options);
            SeedData(context);
            return context;
        }
        private Mapper ConfigureAutoMapper()
        {
            var productProfile = new ProductProfiler();
            var configuration = new MapperConfiguration(config => config.AddProfile(productProfile));
            var mapper = new Mapper(configuration);
            return mapper;
        }
        private void SeedData(ProductsDbContext context)
        {
            for (int i = 1; i <= 10; i++)
            {
                var product = new Product()
                {
                    Id = i,
                    Name = $"Product No {i}",
                    Description = $"Prodcut No {i} Description",
                    Inventory = i * 10,
                    Price = (decimal)(i * 3.52)
                };
                context.Products.Add(product);
            }
            context.SaveChanges();
        }
        private ProductsProvider ConfigureProductsProvider(string databaseName)
        {
            var context = ConfigureDatabase(databaseName);
            var mapper = ConfigureAutoMapper();
            var productsProvider = new ProductsProvider(context, null, mapper);
            return productsProvider;
        }

        [Fact]
        public async Task GetProductsReturnAllProducts()
        {
            var provider = ConfigureProductsProvider(nameof(GetProductsReturnAllProducts));
            var response = await provider.GetProductsAsync();
            Assert.True(response.IsSuccess);
            Assert.Equal(10, response.products.Count());
            Assert.Null(response.ErrorMessage);
        }

        [Fact]
        public async Task GetProductReturnProductByValidID()
        {
            var provider = ConfigureProductsProvider(nameof(GetProductReturnProductByValidID));
            var response = await provider.GetProductAsync(1);
            Assert.True(response.IsSuccess);
            Assert.NotNull(response.product);
            Assert.True(response.product.Id == 1);
            Assert.Null(response.ErrorMessage);
        }

        [Fact]
        public async Task GetProductReturnNullByUnvalidID()
        {
            var provider = ConfigureProductsProvider(nameof(GetProductReturnNullByUnvalidID));
            var response = await provider.GetProductAsync(-5);
            Assert.False(response.IsSuccess);
            Assert.Null(response.product);
            Assert.NotNull(response.ErrorMessage);
        }
    }
}
