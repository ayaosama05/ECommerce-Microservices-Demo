using ECommerce.Api.Customers.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Interfaces
{
    public interface ICustomersProvider
    {
        Task<(bool IsSuccess,IEnumerable<CustomerDto> customers,string ErrorMessage)> GetCustomersAsync();
        Task<(bool IsSuccess, CustomerDto customer, string ErrorMessage)> GetCustomerAsync(int id);
    }
}
