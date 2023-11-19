using AutoMapper;
using AutoMapper.Internal.Mappers;
using ECommerce.Api.Customers.Data;
using ECommerce.Api.Customers.Profiles;
using ECommerce.Api.Customers.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Api.Tests
{
    public class CustomerServiceTest
    {
        [Fact]
        public async Task GetCustomersReturnAllCustomers()
        {
            var provider = ConfigureProvider(nameof(GetCustomersReturnAllCustomers));
            var response = await provider.GetCustomersAsync();
            Assert.True(response.IsSuccess);
            Assert.True(response.customers.Any());
            Assert.Null(response.ErrorMessage);
        }
        [Fact]
        public async Task GetCustomerReturnCustomerByValidID()
        {
            var provider = ConfigureProvider(nameof(GetCustomerReturnCustomerByValidID));
            var response = await provider.GetCustomerAsync(2);
            Assert.True(response.IsSuccess);
            Assert.True(response.customer.Id == 2);
            Assert.Null(response.ErrorMessage);
        }
        [Fact]
        public async Task GetCustomerReturnNullByUnvalidID()
        {
            var provider = ConfigureProvider(nameof(GetCustomerReturnNullByUnvalidID));
            var response = await provider.GetCustomerAsync(-2);
            Assert.False(response.IsSuccess); // Assert.True(response.IsSuccess);
            Assert.Null(response.customer);
            Assert.NotNull(response.ErrorMessage);
        }
        private CustomersProvider ConfigureProvider(string databaseName)
        {
            var context = ConfigureDatabase(databaseName);
            var mapper = ConfigureAutoMapper();
            var provider = new CustomersProvider(context,mapper, null);
            return provider;
        }
        private Mapper ConfigureAutoMapper()
        {
            var profile = new CustomerProfile();
            var configuration = new MapperConfiguration(config => config.AddProfile(profile));
            var mapper = new Mapper(configuration);
            return mapper;
        }
        private CustomersDbContext ConfigureDatabase(string databaseName)
        {
            var options = new DbContextOptionsBuilder<CustomersDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            var context = new CustomersDbContext(options);
            SeedData(context);
            return context;
        }

        private void SeedData(CustomersDbContext context)
        {
            for (int i = 1; i < 5; i++)
            {
                var customer = new Customer()
                {
                    Id = i,
                    Name = $"Customer {i}",
                    Address = $"Address Building No. {i}"
                };
                context.Customers.Add(customer);
            }
            context.SaveChanges();
        }
    }
}
