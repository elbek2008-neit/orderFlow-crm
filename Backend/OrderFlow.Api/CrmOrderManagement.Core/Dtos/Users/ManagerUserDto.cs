using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmOrderManagement.Core.Dtos.Users
{
    public class ManagerUserDto : BaseUserDto
    {
        public int AssignedClientsCount { get; set; }

        public decimal MonthlySalesTotal { get; set; }
    }
}
