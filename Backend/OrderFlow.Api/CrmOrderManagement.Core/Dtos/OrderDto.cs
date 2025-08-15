using CrmOrderManagement.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmOrderManagement.Core.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public int ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public IEnumerable<OrderProductDto> Products { get; set; } = new List<OrderProductDto>();
    }
    public class CreateOrderDto
    {
        [Required]
        public int ClientId { get; set; }

        [Required]
        public int UserId { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; } = string.Empty;

        public IEnumerable<OrderProductCreateDto> Products { get; set; } = new List<OrderProductCreateDto>();
    }

    public class UpdateOrderDto
    {
        [MaxLength(1000)]
        public string Notes { get; set; } = string.Empty;

        public OrderStatus Status { get; set; }
    }

    public class OrderProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductSKU { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class OrderProductCreateDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
