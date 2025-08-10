using Microsoft.EntityFrameworkCore;
using CrmOrderManagement.Core.Entities;
using CrmOrderManagement.Core.Enums;
using CrmOrderManagement.Core.Interfaces;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using CrmOrderManagement.Infrastructure.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using CrmOrderManagement.Core;


namespace CrmOrderManagement.Infrastructure.Seeders
{
    public class DataSeeder
    {
        private readonly CrmDbContext _context;
        private readonly IPasswordService _passwordService;

        public DataSeeder(CrmDbContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        public async Task SeedAsync()
        {
            // Создаем базу данных если не существует
            await _context.Database.EnsureCreatedAsync();

            // Заполняем данные в правильном порядке
            await SeedRolesAsync();
            await SeedUsersAsync();
            await SeedWarehousesAsync();
            await SeedProductsAsync();
            await SeedClientsAsync();
            await SeedWarehouseProductsAsync();
            await SeedOrdersAsync();

        }

        private async Task SeedRolesAsync()
        {
            if (!_context.Roles.Any())
            {
                var roles = new[]
                {
                    new Role
                    {
                        Name = "Admin",
                        Description = "Полный доступ к системе"
                    },
                    new Role
                    {
                        Name = "Manager",
                        Description = "Управление заказами и клиентами"
                    },
                    new Role
                    {
                        Name = "Operator",
                        Description = "Создание и обработка заказов"
                    }
                };

                _context.Roles.AddRange(roles);
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedUsersAsync()
        {
            if (!_context.Users.Any())
            {
                if (!_context.Users.Any())
                {
                    var adminRole = await _context.Roles.FirstAsync(r => r.Name == "Admin");
                    var managerRole = await _context.Roles.FirstAsync(r => r.Name == "Manager");
                    var operatorRole = await _context.Roles.FirstAsync(r => r.Name == "Operator");

                    var users = new[]
                    {
                        new User
                        {
                            UserName = "admin",
                            Email = "admin@crm.com",
                            PasswordHash = _passwordService.HashPassword("Admin123!"),
                            FirstName = "Администратор",
                            LastName = "Системы",
                            IsActive = true,
                            CreatedAt = DateTime.UtcNow
                        },
                        new User
                        {
                            UserName = "manager",
                            Email = "manager@crm.com",
                            PasswordHash = _passwordService.HashPassword("Manager123!"),
                            FirstName = "Иван",
                            LastName = "Менеджеров",
                            IsActive = true,
                            CreatedAt = DateTime.UtcNow
                        },
                        new User
                        {
                            UserName = "operator",
                            Email = "operator@crm.com",
                            PasswordHash = _passwordService.HashPassword("Operator123!"),
                            FirstName = "Мария",
                            LastName = "Операторова",
                            IsActive = true,
                            CreatedAt = DateTime.UtcNow
                        },
                    };

                    _context.Users.AddRange(users);
                    await _context.SaveChangesAsync();

                    // Назначаем роли пользователям
                    var userRoles = new[]
                    {
                        new UserRole { UserId = users[0].Id, RoleId = adminRole.Id },
                        new UserRole { UserId = users[1].Id, RoleId = managerRole.Id },
                        new UserRole { UserId = users[2].Id, RoleId = operatorRole.Id }
                    };

                    _context.UserRoles.AddRange(userRoles);
                    await _context.SaveChangesAsync();
                }
            }
        }

        private async Task SeedWarehousesAsync()
        {
            if (!_context.Warehauses.Any())
            {
                var warehouses = new[]
                {
                    new Warehause
                    {
                        Name = "Центральный склад",
                        Address = "Москва, ул. Складская, 1",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Warehause
                    {
                        Name = "Региональный склад СПб",
                        Address = "Санкт-Петербург, пр. Складской, 25",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Warehause
                    {
                        Name = "Склад Екатеринбург",
                        Address = "Екатеринбург, ул. Промышленная, 10",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    }
                };

                _context.Warehauses.AddRange(warehouses);
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedProductsAsync()
        {
            if (!_context.Products.Any())
            {
                var products = new[]
                {
                    new Product
                    {
                        Name = "iPhone 15 Pro",
                        Description = "Новый iPhone 15 Pro с камерой 48MP, титановый корпус",
                        SKU = "IPHONE15PRO",
                        Price = 89999.00m,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Name = "Samsung Galaxy S24 Ultra",
                        Description = "Флагманский смартфон Samsung с S Pen",
                        SKU = "GALAXY_S24_ULTRA",
                        Price = 79999.00m,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Name = "MacBook Air M3",
                        Description = "Ультрабук Apple MacBook Air с процессором M3",
                        SKU = "MBA_M3_13",
                        Price = 119999.00m,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Name = "Dell XPS 13",
                        Description = "Ультрабук Dell XPS 13 с процессором Intel",
                        SKU = "DELL_XPS13",
                        Price = 89999.00m,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Name = "iPad Pro 12.9",
                        Description = "Планшет Apple iPad Pro с дисплеем 12.9 дюймов",
                        SKU = "IPAD_PRO_129",
                        Price = 69999.00m,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Name = "AirPods Pro",
                        Description = "Беспроводные наушники Apple с активным шумоподавлением",
                        SKU = "AIRPODS_PRO",
                        Price = 24999.00m,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Name = "Apple Watch Series 9",
                        Description = "Умные часы Apple Watch Series 9",
                        SKU = "WATCH_S9",
                        Price = 34999.00m,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Name = "Sony WH-1000XM5",
                        Description = "Беспроводные наушники Sony с шумоподавлением",
                        SKU = "SONY_WH1000XM5",
                        Price = 29999.00m,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    }
                };

                _context.Products.AddRange(products);
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedClientsAsync()
        {
            if (!_context.Clients.Any())
            {
                var clients = new[]
                {
                    new Client
                    {
                        CompanyName = "ООО \"ТехноМир\"",
                        ContactPerson = "Петров Алексей Владимирович",
                        Email = "a.petrov@technomir.ru",
                        Phone = "+7 (499) 123-45-67",
                        Address = "Москва, ул. Тверская, 10, офис 501",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-60)
                    },
                    new Client
                    {
                        CompanyName = "ИП Сидорова М.А.",
                        ContactPerson = "Сидорова Мария Андреевна",
                        Email = "maria@sidorova-shop.ru",
                        Phone = "+7 (812) 987-65-43",
                        Address = "Санкт-Петербург, Невский пр., 88, офис 12",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-45)
                    },
                    new Client
                    {
                        CompanyName = "ООО \"Электроника Плюс\"",
                        ContactPerson = "Козлов Дмитрий Сергеевич",
                        Email = "d.kozlov@electronika-plus.ru",
                        Phone = "+7 (343) 555-12-34",
                        Address = "Екатеринбург, ул. Ленина, 25, офис 203",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-30)
                    },
                    new Client
                    {
                        CompanyName = "ООО \"Гаджеты и Ко\"",
                        ContactPerson = "Волкова Анна Игоревна",
                        Email = "a.volkova@gadgets-co.ru",
                        Phone = "+7 (383) 777-89-01",
                        Address = "Новосибирск, пр. Красный, 45, офис 310",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-20)
                    },
                    new Client
                    {
                        CompanyName = "ИП Морозов С.В.",
                        ContactPerson = "Морозов Сергей Викторович",
                        Email = "s.morozov@personal-shop.ru",
                        Phone = "+7 (843) 333-22-11",
                        Address = "Казань, ул. Баумана, 58, офис 7",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-15)
                    },
                    new Client
                    {
                        CompanyName = "ООО \"Цифровые Решения\"",
                        ContactPerson = "Николаев Павел Александрович",
                        Email = "p.nikolaev@digital-solutions.ru",
                        Phone = "+7 (861) 444-55-66",
                        Address = "Краснодар, ул. Красная, 122, офис 45",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-10)
                    }
                };

                _context.Clients.AddRange(clients);
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedWarehouseProductsAsync()
        {
            if (!_context.WarehauseProducts.Any())
            {
                var warehouses = await _context.Warehauses.ToListAsync();
                var products = await _context.Products.ToListAsync();
                var random = new Random();

                var warehauseProducts = new List<WarehauseProduct>();

                foreach (var warehouse in warehouses)
                {
                    foreach (var product in products)
                    {
                        var quantity = random.Next(5, 100); // От 5 до 100 штук на складе

                        warehauseProducts.Add(new WarehauseProduct
                        {
                            WarehauseId = warehouse.Id,
                            ProductId = product.Id,
                            Quantity = quantity,
                            LastUpdated = DateTime.UtcNow.AddDays(-random.Next(1, 30))
                        });
                    }
                }

                _context.WarehauseProducts.AddRange(warehauseProducts);
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedOrdersAsync()
        {
            if (!_context.Orders.Any())
            {
                var clients = await _context.Clients.ToListAsync();
                var users = await _context.Users.ToListAsync();
                var products = await _context.Products.ToListAsync();
                var random = new Random();

                var orders = new List<Order>();
                var orderProducts = new List<OrderProduct>();

                for (int i = 1; i <= 20; i++)
                {
                    var client = clients[random.Next(clients.Count)];
                    var user = users[random.Next(users.Count)];
                    var orderDate = DateTime.UtcNow.AddDays(-random.Next(1, 90));

                    // Определяем статус заказа (более старые заказы чаще имеют финальные статусы)
                    var daysSinceCreation = (DateTime.UtcNow - orderDate).Days;
                    OrderStatus status;

                    if (daysSinceCreation > 30)
                    {
                        status = (OrderStatus)random.Next(3, 6); // Processing, Shipped, Delivered, Cancelled
                    }
                    else if (daysSinceCreation > 7)
                    {
                        status = (OrderStatus)random.Next(1, 5); // Confirmed, Processing, Shipped, Delivered
                    }
                    else
                    {
                        status = (OrderStatus)random.Next(0, 3); // Draft, Confirmed, Processing
                    }

                    var order = new Order
                    {
                        OrderNumber = $"ORD-{orderDate.Year}-{i:D4}",
                        ClientId = client.Id,
                        UserId = user.Id,
                        Status = status,
                        Notes = i % 4 == 0 ? GetRandomOrderNote() : string.Empty,
                        CreatedAt = orderDate,
                        UpdatedAt = status != OrderStatus.Draft ? orderDate.AddHours(random.Next(1, 48)) : null
                    };

                    orders.Add(order);
                }

                _context.Orders.AddRange(orders);
                await _context.SaveChangesAsync();

                // Добавляем товары к заказам
                foreach (var order in orders)
                {
                    var productsInOrder = random.Next(1, 5); // 1-4 товара в заказе
                    var selectedProducts = products.OrderBy(x => random.Next()).Take(productsInOrder).ToList();
                    decimal totalAmount = 0;

                    foreach (var product in selectedProducts)
                    {
                        var quantity = random.Next(1, 10);
                        var unitPrice = product.Price;
                        var totalPrice = quantity * unitPrice;
                        totalAmount += totalPrice;

                        orderProducts.Add(new OrderProduct
                        {
                            OrderId = order.Id,
                            ProductId = product.Id,
                            Quantity = quantity,
                            UnitPrice = unitPrice
                        });
                    }

                    order.TotalAmount = totalAmount;
                }

                _context.OrderProducts.AddRange(orderProducts);
                await _context.SaveChangesAsync();

                // Логирование результатов
                Console.WriteLine("✅ База данных успешно заполнена тестовыми данными!");
                Console.WriteLine($"📊 Создано записей:");
                Console.WriteLine($"   Пользователи: {_context.Users.Count()}");
                Console.WriteLine($"   Роли: {_context.Roles.Count()}");
                Console.WriteLine($"   Клиенты: {_context.Clients.Count()}");
                Console.WriteLine($"   Товары: {_context.Products.Count()}");
                Console.WriteLine($"   Склады: {_context.Warehauses.Count()}");
                Console.WriteLine($"   Заказы: {_context.Orders.Count()}");
                Console.WriteLine($"   Позиции заказов: {_context.OrderProducts.Count()}");
                Console.WriteLine($"   Остатки на складах: {_context.WarehauseProducts.Count()}");
            }
        }

        private static string GetRandomOrderNote()
        {
            var notes = new[]
            {
                "Срочная доставка до 18:00",
                "Требуется согласование с руководством",
                "Постоянный клиент, предоставить скидку",
                "Доставка в офис, звонить за час",
                "Проверить наличие гарантийных документов",
                "Клиент запросил дополнительную упаковку",
                "Оплата по факту получения товара"
            };

            var random = new Random();
            return notes[random.Next(notes.Length)];
        }
    }

}
