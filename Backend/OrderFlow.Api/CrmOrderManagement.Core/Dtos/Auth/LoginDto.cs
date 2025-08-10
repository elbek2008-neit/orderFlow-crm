using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmOrderManagement.Core.Dtos.Auth
{
    public class LoginDto
    {
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
