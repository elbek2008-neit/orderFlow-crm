using CrmOrderManagement.Core.Interfaces;
using CrmOrderManagement.Infrastructure.EF;
using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CrmOrderManagement.Core.Dtos.Auth;
using CrmOrderManagement.Core.AuthEntities;
using CrmOrderManagement.Core.Entities;
using Microsoft.IdentityModel.Tokens;

namespace CrmOrderManagement.Services.Service
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwtService;
        private readonly CrmDbContext _context;
        private readonly IPasswordService _passwordService;

        public AuthService(IJwtService jwtService, CrmDbContext context, IPasswordService passwordService) 
        {
            _context = context; 
            _jwtService = jwtService;
            _passwordService = passwordService;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto) 
        {
            var user = await _context.Users
                 .Include(u => u.UserRoles)
                 .ThenInclude(ur => ur.Role)
                 .FirstOrDefaultAsync(u =>
                     u.UserName == dto.Username);
                     

            if (user == null || !_passwordService.VerifyPassword(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid username or password");

            var claims = new UserClaims
            {
                UserId = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Roles = user.UserRoles.Select(r => r.Role.Name).ToList()
            };

            return new AuthResponseDto
            {
                AccessToken = _jwtService.GenerateAccessToken(claims),
                RefreshToken = _jwtService.GenerateRefreshToken(),
                ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                Roles = claims.Roles
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                throw new InvalidOperationException("Email already exists");

            var user = new User
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = _passwordService.HashPassword(registerDto.Password),
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Добавляем роли
            if (registerDto.Roles != null && registerDto.Roles.Count > 0)
            {
                var roles = await _context.Roles
                    .Where(r => registerDto.Roles.Contains(r.Name))
                    .ToListAsync();

                foreach (var role in roles)
                {
                    _context.UserRoles.Add(new UserRole
                    {
                        UserId = user.Id,
                        RoleId = role.Id
                    });
                }

                await _context.SaveChangesAsync();
            }

            var claims = new UserClaims
            {
                UserId = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Roles = registerDto.Roles ?? new List<string>()
            };

            return new AuthResponseDto
            {
                AccessToken = _jwtService.GenerateAccessToken(claims),
                RefreshToken = _jwtService.GenerateRefreshToken(),
                ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                Roles = claims.Roles
            };
        }

        public Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
                throw new SecurityTokenException("Invalid token");

            var userClaims = _jwtService.GetUserClaimsFromToken(request.AccessToken);
            if (userClaims == null)
                throw new SecurityTokenException("Invalid token claims");

            return Task.FromResult(new AuthResponseDto
            {
                AccessToken = _jwtService.GenerateAccessToken(userClaims),
                RefreshToken = _jwtService.GenerateRefreshToken(),
                ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                Roles = userClaims.Roles
            });
        }

        public async Task<UserDto?> GetCurrentUserAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = user.UserRoles.Select(r => r.Role.Name).ToList(),
                IsActive = user.IsActive
            };
        }


    }
}
