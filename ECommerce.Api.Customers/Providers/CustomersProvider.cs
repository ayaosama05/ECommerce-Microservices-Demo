using AutoMapper;
using ECommerce.Api.Customers.Data;
using ECommerce.Api.Customers.Dtos;
using ECommerce.Api.Customers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly CustomersDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<CustomersProvider> logger;

        public CustomersProvider(CustomersDbContext context, IMapper mapper, ILogger<CustomersProvider> logger)
        {
            this.dbContext = context;
            this.mapper = mapper;
            this.logger = logger;

            SeedCustomers();
        }

        private void SeedCustomers()
        {
            if (!dbContext.Customers.Any())
            {
                List<Customer> customers = new List<Customer>
                {
                    new Customer() { Id = 1,Name = "Aya Osama",Address = "El-Mahalla El-Kubra"},
                    new Customer() { Id = 2,Name = "Mohamed Fawzy",Address = "Banha"},
                    new Customer() { Id = 3,Name = "Nour Amam",Address = "Ahmed El shaarawy street El-Mahalla El-Kubra"},
                    new Customer() { Id = 4,Name = "Ahmed Mohamed",Address = "El Mansoura, El dakahlia governorate"},
                    new Customer() { Id = 5,Name = "Amir Ibrahim",Address = "Giza, 6th of october"},
                    new Customer() { Id = 6,Name = "Ahmed Abdelsalam",Address = "El-Zawia El-Hamra"},
                };

                dbContext.Customers.AddRange(customers);
                dbContext.SaveChanges();
            }
        }
        public async Task<(bool IsSuccess, IEnumerable<CustomerDto> customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                logger?.LogInformation($"Querying Customers");
                var customers = await dbContext.Customers.ToListAsync();
                if(customers != null && customers.Any())
                {
                    logger?.LogInformation($"{customers.Count} products found");
                    var result = mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerDto>>(customers);
                    return (true, result, null);
                }
                return (false, null, "No Customers found !");
            }
            catch (Exception ex)
            {

                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, CustomerDto customer, string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                logger?.LogInformation($"Querying Customer");
                var customer = await dbContext.Customers.FirstOrDefaultAsync(a => a.Id == id);
                if (customer != null)
                {
                    logger?.LogInformation($"Customer found");
                    var result = mapper.Map<Customer, CustomerDto>(customer);
                    return (true, result, null);
                }
                return (false, null, "Customer Not found !");
            }
            catch (Exception ex)
            {

                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
