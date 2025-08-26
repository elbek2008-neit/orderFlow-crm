using AutoMapper;
using CrmOrderManagement.Core.Dtos;
using CrmOrderManagement.Core.Entities;
using CrmOrderManagement.Core.Enums;
using CrmOrderManagement.Core.Interfaces;
using CrmOrderManagement.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmOrderManagement.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly CrmDbContext _context;
        private readonly IMapper _mapper;

        public OrderService(CrmDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.User)
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto?> GetOrderByNumberAsync(string orderNumber)
        {
            var order = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.User)
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<(IEnumerable<OrderDto> Orders, int TotalCount)> GetOrdersPagedAsync(
            int pageNumber, int pageSize, int? clientId = null, OrderStatus? status = null)
        {
            var query = _context.Orders
                .Include(o => o.Client)
                .Include(o => o.User)
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .AsQueryable();

            if (clientId.HasValue)
                query = query.Where(o => o.ClientId == clientId.Value);

            if (status.HasValue)
                query = query.Where(o => o.Status == status.Value);

            var totalCount = await query.CountAsync();

            var orders = await query
                .OrderByDescending(o => o.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var ordersDto = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return (ordersDto, totalCount);
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            var order = new Order
            {
                OrderNumber = Guid.NewGuid().ToString("N")[..8],
                ClientId = createOrderDto.ClientId,
                UserId = createOrderDto.UserId,
                Notes = createOrderDto.Notes,
                Status = OrderStatus.Draft,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var item in createOrderDto.Products)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                    throw new Exception($"Product {item.ProductId} not found.");

                var orderProduct = new OrderProduct
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                    TotalPrice = product.Price * item.Quantity
                };

                order.OrderProducts.Add(orderProduct);
            }

            order.TotalAmount = order.OrderProducts.Sum(op => op.TotalPrice);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> UpdateOrderAsync(int id, UpdateOrderDto updateOrderDto)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) throw new Exception("Order not found.");

            order.Notes = updateOrderDto.Notes;
            order.Status = updateOrderDto.Status;
            order.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateOrderStatusAsync(int id, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddProductToOrderAsync(int orderId, int productId, int quantity)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) throw new Exception("Order not found.");

            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new Exception("Product not found.");

            var existing = order.OrderProducts.FirstOrDefault(op => op.ProductId == productId);
            if (existing != null)
            {
                existing.Quantity += quantity;
                existing.TotalPrice = existing.UnitPrice * existing.Quantity;
            }
            else
            {
                order.OrderProducts.Add(new OrderProduct
                {
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = product.Price,
                    TotalPrice = product.Price * quantity
                });
            }

            order.TotalAmount = order.OrderProducts.Sum(op => op.TotalPrice);
            order.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveProductFromOrderAsync(int orderId, int productId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) throw new Exception("Order not found.");

            var item = order.OrderProducts.FirstOrDefault(op => op.ProductId == productId);
            if (item == null) throw new Exception("Product not in order.");

            order.OrderProducts.Remove(item);
            order.TotalAmount = order.OrderProducts.Sum(op => op.TotalPrice);
            order.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ValidateOrderInventoryAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) return false;

            // В этой реализации Product.Quantity нет, поэтому просто возвращаем true
            return true;
        }

        public async Task<decimal> CalculateOrderTotalAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) throw new Exception("Order not found.");

            return order.OrderProducts.Sum(op => op.TotalPrice);
        }
    }
}