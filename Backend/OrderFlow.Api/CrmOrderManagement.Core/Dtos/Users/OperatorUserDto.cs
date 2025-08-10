using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmOrderManagement.Core.Dtos.Users
{
    public class OperatorUserDto : BaseUserDto
    {
        public int ProcessedOrdersCount { get; set; }

        public DateTime LastOrderProcessedAt { get; set; }
    }
}
