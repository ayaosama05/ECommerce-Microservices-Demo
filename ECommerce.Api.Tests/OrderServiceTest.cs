using AutoMapper;
using ECommerce.Api.Customers.Data;
using ECommerce.Api.Customers.Profiles;
using ECommerce.Api.Customers.Providers;
using ECommerce.Api.Orders.Data;
using ECommerce.Api.Orders.Interfaces;
using ECommerce.Api.Orders.Profiles;
using ECommerce.Api.Orders.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Api.Tests
{
    public class OrderServiceTest
    {
        [Fact]
        public async Task GetOrderReturnOrderByCustomerID()
        {
            var provider = ConfigureProvider(nameof(GetOrderReturnOrderByCustomerID));
            var response = await provider.GetOrdersAsync(1);
            Assert.True(response.IsSuccess);
            Assert.Equal(4, response.Orders.Count());
            Assert.True(response.Orders.Any());
            Assert.Null(response.ErrorMessage);
        }

        private OrdersProvider ConfigureProvider(string databaseName)
        {
            var context = ConfigureDatabase(databaseName);
            var mapper = ConfigureAutoMapper();
            var provider = new OrdersProvider(context, mapper, null);
            return provider;
        }

        private OrderDbContext ConfigureDatabase(string databaseName)
        {
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            var context = new OrderDbContext(options);
            SeedData(context);
            return context;
        }

        private void SeedData(OrderDbContext context)
        {
            for (int i = 1; i < 5; i++)
            {
                var order = new Order() {
                    Id = i,
                    CustomerId = 1,
                    OrderDate = DateTime.Now.AddDays(-i) ,
                    Items = new List<OrderItem>() { 
                        new OrderItem() { OrderId = 1 , ProductId = 1,Quanity = 2 , UnitPrice = (decimal)(i*10.5)},
                        new OrderItem() { OrderId = 1 , ProductId = 2,Quanity = 4 , UnitPrice = (decimal)(i*14.5)}, 
                        new OrderItem() { OrderId = 2 , ProductId = 3,Quanity = 5 , UnitPrice = (decimal)(i*7.2)},
                        new OrderItem() { OrderId = 3 , ProductId = 3,Quanity = 5 , UnitPrice = (decimal)(i*8.5)},
                        new OrderItem() { OrderId = 3 , ProductId = 2,Quanity = 10 , UnitPrice = (decimal)(i*22.45)},
                    },
                    Total = 10 };
                context.Orders.Add(order);
            }
            context.SaveChanges();
        }

        private Mapper ConfigureAutoMapper()
        {
            var profile = new OrderProfile();
            var configuration = new MapperConfiguration(config => config.AddProfile(profile));
            var mapper = new Mapper(configuration);
            return mapper;
        }
    }
}
