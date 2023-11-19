using AutoMapper;
using ECommerce.Api.Orders.Data;
using ECommerce.Api.Orders.Dtos;
using ECommerce.Api.Orders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrderDbContext context;
        private readonly IMapper mapper;
        private readonly ILogger<OrdersProvider> logger;

        public OrdersProvider(OrderDbContext context, IMapper mapper, ILogger<OrdersProvider> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;

            SeedOrders();
        }

        private void SeedOrders()
        {
            if (!context.Orders.Any())
            {
                var orders = new List<Order>()
                {
                    new Order()
                    { Id = 1
                    , CustomerId = 1
                    , OrderDate = DateTime.Now.AddDays(-3)
                    , Total = 100
                    , Items = new List<OrderItem>(){
                        new OrderItem() { OrderId = 1 , ProductId = 1 , Quanity = 10 , UnitPrice = 10},
                        new OrderItem() { OrderId = 1 , ProductId = 2 , Quanity = 10 , UnitPrice = 10},
                        new OrderItem() { OrderId = 1 , ProductId = 3 , Quanity = 10 , UnitPrice = 10},
                        new OrderItem() { OrderId = 2 , ProductId = 1 , Quanity = 10 , UnitPrice = 10},
                        new OrderItem() { OrderId = 3 , ProductId = 2 , Quanity = 1 , UnitPrice = 100},
                        }
                    },
                    new Order()
                    { Id = 2
                    , CustomerId = 2
                    , OrderDate = DateTime.Now.AddDays(-1)
                    , Total = 100
                    , Items = new List<OrderItem>(){
                        new OrderItem() { OrderId = 1 , ProductId = 1 , Quanity = 10 , UnitPrice = 10},
                        new OrderItem() { OrderId = 1 , ProductId = 2 , Quanity = 10 , UnitPrice = 10},
                        new OrderItem() { OrderId = 1 , ProductId = 3 , Quanity = 10 , UnitPrice = 10},
                        new OrderItem() { OrderId = 2 , ProductId = 1 , Quanity = 2 , UnitPrice = 100},
                        new OrderItem() { OrderId = 3 , ProductId = 2 , Quanity = 1 , UnitPrice = 100},
                        }
                    },
                    new Order()
                    { Id = 3
                    , CustomerId = 3
                    , OrderDate = DateTime.Now.AddHours(-2)
                    , Total = 100
                    , Items = new List<OrderItem>(){
                        new OrderItem() { OrderId = 1 , ProductId = 1 , Quanity = 10 , UnitPrice = 10},
                        new OrderItem() { OrderId = 1 , ProductId = 2 , Quanity = 10 , UnitPrice = 10},
                        new OrderItem() { OrderId = 1 , ProductId = 3 , Quanity = 10 , UnitPrice = 10},
                        new OrderItem() { OrderId = 2 , ProductId = 1 , Quanity = 10 , UnitPrice = 10},
                        new OrderItem() { OrderId = 3 , ProductId = 2 , Quanity = 1 , UnitPrice = 100},
                        }
                    }
                };
                context.Orders.AddRange(orders);
                context.SaveChanges();
            }
        }
        public async Task<(bool IsSuccess, IEnumerable<OrderDto> Orders, string ErrorMessage)> GetOrdersAsync(int CustomerId)
        {
            try
            {
                logger?.LogInformation("Start Order Querying");
                var orders = await context.Orders
                    .Where(a => a.CustomerId == CustomerId)
                    .Include(a => a.Items)
                    .ToListAsync();

                if(orders != null && orders.Any())
                {
                    logger?.LogInformation($"Total Orders Found for customer {CustomerId} is {orders.Count}");
                    var result = mapper.Map<IEnumerable<Order>, IEnumerable<OrderDto>>(orders);
                    return (true, result, null);
                }

                return (false, null, "No Orders Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
