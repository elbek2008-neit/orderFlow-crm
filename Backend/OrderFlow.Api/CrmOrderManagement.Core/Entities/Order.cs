using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrmOrderManagement.Core.Enums;


namespace CrmOrderManagement.Core.Entities
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string OrderNumber { get; set; } = string.Empty;

        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;  

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Draft;

        [MaxLength(1000)]
        public string Notes { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set;}

        public ICollection<OrderProduct> OrderProduct { get; set; } = new List<OrderProduct>();
    }
}
