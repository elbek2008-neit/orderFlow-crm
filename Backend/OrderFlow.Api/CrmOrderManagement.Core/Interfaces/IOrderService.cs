using CrmOrderManagement.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrmOrderManagement.Core.Dtos;

namespace CrmOrderManagement.Core.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto?> GetOrderByIdAsync(int id);
        Task<OrderDto?> GetOrderByNumberAsync(string orderNumber);
        Task<(IEnumerable<OrderDto> Orders, int TotalCount)> GetOrdersPagedAsync(int pageNumber, int pageSize, int? clientId = null, OrderStatus? status = null);
        Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto);
        Task<OrderDto> UpdateOrderAsync(int id, UpdateOrderDto updateOrderDto);
        Task<bool> DeleteOrderAsync(int id);
        Task<bool> UpdateOrderStatusAsync(int id, OrderStatus status);
        Task<bool> AddProductToOrderAsync(int orderId, int productId, int quantity);
        Task<bool> RemoveProductFromOrderAsync(int orderId, int productId);
        Task<bool> ValidateOrderInventoryAsync(int orderId);
        Task<decimal> CalculateOrderTotalAsync(int orderId);
    }
}
