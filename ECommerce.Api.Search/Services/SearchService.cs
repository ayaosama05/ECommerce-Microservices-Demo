using ECommerce.Api.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrdersService ordersService;
        private readonly IProductsService productsService;
        private readonly ICustomersService customersService;

        public SearchService(IOrdersService ordersService,IProductsService productsService,ICustomersService customersService)
        {
            this.ordersService = ordersService;
            this.productsService = productsService;
            this.customersService = customersService;
        }
        public async Task<(bool IsSucess, dynamic result)> SearchAsync(int customerId)
        {
            var productResponse = await productsService.GetProductsAsync();

            var orderResponse = await ordersService.GetOrdersAsync(customerId);
            var customerResponse = await customersService.GetCustomerAsync(customerId);    
            if (orderResponse.IsSuccess)
            {
                foreach(var Order in orderResponse.Orders)
                {
                    foreach(var item in Order.Items)
                    {
                        item.ProductName = productResponse.IsSuccess ?
                            productResponse.products.FirstOrDefault(a => a.Id == item.ProductId).Name : 
                            "N/A";
                    }
                }
                return (true, new
                {
                    Customer= customerResponse.IsSuccess ? customerResponse.Customer : null,
                    Orders = orderResponse.Orders
                });
            }
            return (false, null);
           // await Task.Delay(1);
           // return (true, new { Message = "Response From Search Service" });
        }
    }
}
