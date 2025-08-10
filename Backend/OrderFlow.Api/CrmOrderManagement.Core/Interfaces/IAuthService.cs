using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CrmOrderManagement.Core.AuthEntities;
using CrmOrderManagement.Core.Dtos.Auth;

namespace CrmOrderManagement.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequest dto);
        Task<UserDto> GetCurrentUserAsync(int userId);
    }
}
