using AutoMapper;
using CrmOrderManagement.Core.Dtos;
using CrmOrderManagement.Core.Entities;
using CrmOrderManagement.Core.Enums;
using CrmOrderManagement.Core.Interfaces;
using CrmOrderManagement.Infrastructure.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CrmOrderManagement.Services.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = (await _unitOfWork.Users.FindAsync(u => u.Email == email)).FirstOrDefault();
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            var user = (await _unitOfWork.Users.FindAsync(u => u.UserName == username)).FirstOrDefault();
            return _mapper.Map<UserDto>(user);
        }

        public async Task<(IEnumerable<UserDto> Users, int TotalCount)> GetUsersPagedAsync(int pageNumber, int pageSize, string? searchTerm = null)
        {
            Expression<Func<User, bool>>? filter = null;

            if (!string.IsNullOrEmpty(searchTerm))
                filter = u => u.UserName.Contains(searchTerm) || u.Email.Contains(searchTerm);

            var (items, totalCount) = await _unitOfWork.Users.GetPagedAsync(pageNumber, pageSize, filter, u => u.Id);

            return (_mapper.Map<IEnumerable<UserDto>>(items), totalCount);
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = _mapper.Map<User>(createUserDto);
            _unitOfWork.Users.Add(user);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null) throw new Exception("User not found");

            _mapper.Map(updateUserDto, user);
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null) return false;

            _unitOfWork.Users.Remove(user);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> AssignRoleAsync(int userId, int roleId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            var role = await _unitOfWork.Roles.GetByIdAsync(roleId);

            if (user == null || role == null) return false;

            user.UserRoles.Add(new UserRole
            {
                UserId = userId,
                RoleId = roleId
            });
            _unitOfWork.Users.Update(user);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRoleAsync(int userId, int roleId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) return false;

            var role = user.UserRoles.FirstOrDefault(r => r.RoleId == roleId);
            if (role != null)
                user.UserRoles.Remove(role);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) return Enumerable.Empty<string>();

            return user.UserRoles.Select(r => r.Role.Name);
        }
    }

}
