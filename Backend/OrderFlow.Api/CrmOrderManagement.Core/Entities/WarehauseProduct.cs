using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmOrderManagement.Core.Entities
{
    public class WarehauseProduct
    {
        public int WarehauseId { get; set; }
        public Warehause Warehause { get; set; } = null;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null;

        public int Quantity { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
