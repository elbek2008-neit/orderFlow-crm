using Microsoft.EntityFrameworkCore;
using CrmOrderManagement.Core.Entities;
using CrmOrderManagement.Core.Enums;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmOrderManagement.Infrastructure.EF
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(CrmDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            // Seed Users
            if (!await context.Users.AnyAsync())
            {
                await SeedUsersAsync(context);
            }

            // Seed Clients
            if (!await context.Clients.AnyAsync())
            {
                await SeedClientsAsync(context);
            }

            // Seed Products
            if (!await context.Products.AnyAsync())
            {
                await SeedProductsAsync(context);
            }

            // Seed Warehouse Products
            if (!await context.WarehauseProducts.AnyAsync())
            {
                await SeedWarehouseProductsAsync(context);
            }

            // Seed Orders
            if (!await context.Orders.AnyAsync())
            {
                await SeedOrdersAsync(context);
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedUsersAsync(CrmDbContext context)
        {
            var users = new[]
            {
            new User
            {
                Id = 1,
                UserName = "admin",
                Email = "admin@company.com",
                PasswordHash = HashPassword("admin123"),
                FirstName = "System",
                LastName = "Administrator",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = 2,
                UserName = "manager1",
                Email = "manager1@company.com",
                PasswordHash = HashPassword("manager123"),
                FirstName = "John",
                LastName = "Manager",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = 3,
                UserName = "manager2",
                Email = "manager2@company.com",
                PasswordHash = HashPassword("manager123"),
                FirstName = "Jane",
                LastName = "Smith",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();

            // Assign roles
            var userRoles = new[]
            {
            new UserRole { UserId = 1, RoleId = 1 }, // Admin
            new UserRole { UserId = 2, RoleId = 2 }, // Manager
            new UserRole { UserId = 3, RoleId = 2 }  // Manager
        };

            await context.UserRoles.AddRangeAsync(userRoles);
        }

        private static async Task SeedClientsAsync(CrmDbContext context)
        {
            var clients = new[]
            {
            new Client
            {
                Id = 1,
                CompanyName = "ABC Corporation",
                ContactPerson = "Michael Johnson",
                Email = "contact@abc-corp.com",
                Phone = "+1-555-0101",
                Address = "123 Business Ave, Suite 100, Business City, BC 12345",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Client
            {
                Id = 2,
                CompanyName = "XYZ Industries",
                ContactPerson = "Sarah Wilson",
                Email = "info@xyz-industries.com",
                Phone = "+1-555-0202",
                Address = "456 Industrial Blvd, Industrial Park, IP 67890",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Client
            {
                Id = 3,
                CompanyName = "TechStart Solutions",
                ContactPerson = "David Chen",
                Email = "hello@techstart.com",
                Phone = "+1-555-0303",
                Address = "789 Innovation Dr, Tech Hub, TH 11111",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

            await context.Clients.AddRangeAsync(clients);
        }

        private static async Task SeedProductsAsync(CrmDbContext context)
        {
            var products = new[]
            {
            new Product
            {
                Id = 1,
                Name = "Laptop Computer",
                Description = "High-performance business laptop",
                SKU = "LAP-001",
                Price = 999.99m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = 2,
                Name = "Wireless Mouse",
                Description = "Ergonomic wireless optical mouse",
                SKU = "MOU-001",
                Price = 29.99m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = 3,
                Name = "Mechanical Keyboard",
                Description = "RGB backlit mechanical keyboard",
                SKU = "KEY-001",
                Price = 149.99m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = 4,
                Name = "Monitor 24inch",
                Description = "Full HD LED monitor",
                SKU = "MON-001",
                Price = 249.99m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = 5,
                Name = "USB-C Hub",
                Description = "Multi-port USB-C hub with HDMI",
                SKU = "HUB-001",
                Price = 79.99m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

            await context.Products.AddRangeAsync(products);
        }

        private static async Task SeedWarehouseProductsAsync(CrmDbContext context)
        {
            var warehouseProducts = new[]
            {
                // Main Warehouse
            new WarehauseProduct { WarehauseId = 1, ProductId = 1, Quantity = 50, LastUpdated = DateTime.UtcNow },
            new WarehauseProduct { WarehauseId = 1, ProductId = 2, Quantity = 200, LastUpdated = DateTime.UtcNow },
            new WarehauseProduct { WarehauseId = 1, ProductId = 3, Quantity = 75, LastUpdated = DateTime.UtcNow },
            new WarehauseProduct { WarehauseId = 1, ProductId = 4, Quantity = 30, LastUpdated = DateTime.UtcNow },
            new WarehauseProduct { WarehauseId = 1, ProductId = 5, Quantity = 100, LastUpdated = DateTime.UtcNow },

                // Secondary Warehouse
            new WarehauseProduct { WarehauseId = 2, ProductId = 1, Quantity = 25, LastUpdated = DateTime.UtcNow },
            new WarehauseProduct { WarehauseId = 2, ProductId = 2, Quantity = 150, LastUpdated = DateTime.UtcNow },
            new WarehauseProduct { WarehauseId = 2, ProductId = 3, Quantity = 40, LastUpdated = DateTime.UtcNow },
            new WarehauseProduct { WarehauseId = 2, ProductId = 4, Quantity = 20, LastUpdated = DateTime.UtcNow },
            new WarehauseProduct { WarehauseId = 2, ProductId = 5, Quantity = 60, LastUpdated = DateTime.UtcNow }
        };

            await context.WarehauseProducts.AddRangeAsync(warehouseProducts);
        }

        private static async Task SeedOrdersAsync(CrmDbContext context)
        {
            var orders = new[]
            {
            new Order
            {
                Id = 1,
                OrderNumber = "ORD-2024-001",
                ClientId = 1,
                UserId = 2,
                TotalAmount = 1179.97m,
                Status = OrderStatus.Confirmed,
                Notes = "Rush order - client needs by end of week",
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            },
            new Order
            {
                Id = 2,
                OrderNumber = "ORD-2024-002",
                ClientId = 2,
                UserId = 3,
                TotalAmount = 479.96m,
                Status = OrderStatus.Processing,
                Notes = "Standard delivery",
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            },
            new Order
            {
                Id = 3,
                OrderNumber = "ORD-2024-003",
                ClientId = 3,
                UserId = 2,
                TotalAmount = 79.99m,
                Status = OrderStatus.Draft,
                Notes = "Waiting for client confirmation",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            }
        };

            await context.Orders.AddRangeAsync(orders);
            await context.SaveChangesAsync();

            // Seed Order Products
            var orderProducts = new[]
            {
            // Order 1
            new OrderProduct { OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 999.99m },
            new OrderProduct { OrderId = 1, ProductId = 2, Quantity = 2, UnitPrice = 29.99m },
            new OrderProduct { OrderId = 1, ProductId = 3, Quantity = 1, UnitPrice = 149.99m },

            // Order 2
            new OrderProduct { OrderId = 2, ProductId = 4, Quantity = 1, UnitPrice = 249.99m },
            new OrderProduct { OrderId = 2, ProductId = 2, Quantity = 3, UnitPrice = 29.99m },
            new OrderProduct { OrderId = 2, ProductId = 3, Quantity = 1, UnitPrice = 149.99m },

            // Order 3
            new OrderProduct { OrderId = 3, ProductId = 5, Quantity = 1, UnitPrice = 79.99m }
        };

            await context.OrderProducts.AddRangeAsync(orderProducts);
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
