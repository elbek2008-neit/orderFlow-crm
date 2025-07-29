using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmOrderManagement.Core.Entities
{
    public class Warehause
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Adress { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
         
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<WarehauseProduct> WarehauseProducts { get; set; } = new List<WarehauseProduct>();
    }
}
