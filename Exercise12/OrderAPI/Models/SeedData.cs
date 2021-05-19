using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace OrderAPI.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new OrderContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<OrderContext>>());
            if (context.Orders.Any())
                return;


            context.Orders.AddRange(
                new Order
                {
                    Customer = "customer1",
                    Products = new List<OrderDetails>
                    {
                        new()
                        {
                            ProductName = "good1_1",
                            Price = 1.23,
                            Amount = 1
                        },
                        new()
                        {
                            ProductName = "good1_2",
                            Price = 2.34,
                            Amount = 3
                        }
                    },
                    UpdatedAt = DateTime.Now
                },
                new Order
                {
                    Customer = "customer2",
                    Products = new List<OrderDetails>
                    {
                        new()
                        {
                            ProductName = "good2_1",
                            Price = 3.45,
                            Amount = 2
                        },
                        new()
                        {
                            ProductName = "good2_2",
                            Price = 4.56,
                            Amount = 4
                        }
                    },
                    UpdatedAt = DateTime.Now
                },
                new Order
                {
                    Customer = "customer3",
                    Products = new List<OrderDetails>
                    {
                        new()
                        {
                            ProductName = "good2_1",
                            Price = 4.56,
                            Amount = 3
                        },
                        new()
                        {
                            ProductName = "good2_2",
                            Price = 5.67,
                            Amount = 5
                        }
                    },
                    UpdatedAt = DateTime.Now
                }
            );
            context.SaveChanges();
        }
    }
}