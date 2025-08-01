using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrmOrderManagement;

namespace CrmOrderManagement.Core.Enums
{
    public enum OrderStatus
    {
        Draft = 0,
        Confirmed = 1,
        Processing = 2,
        Shipped = 3,
        Delivery = 4,
        Cancelled = 5
    }
}
    