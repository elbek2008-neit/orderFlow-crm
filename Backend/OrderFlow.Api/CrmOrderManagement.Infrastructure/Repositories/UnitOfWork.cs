using CrmOrderManagement.Core.Entities;
using CrmOrderManagement.Core.Repositories;
using CrmOrderManagement.Infrastructure.Repositories.IRepositories;
using CrmOrderManagement.Infrastructure.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmOrderManagement.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CrmDbContext _context;

        // Lazy-loaded репозитории
        private IRepository<User>? _users;
        private IRepository<Role>? _roles;
        private IRepository<Client>? _clients;
        private IRepository<Product>? _products;
        private IRepository<Order>? _orders;
        private IRepository<OrderProduct>? _orderProducts;
        private IRepository<Warehause>? _warehauses;
        private IRepository<WarehauseProduct>? _warehauseProducts;
        private IRepository<Auditlog>? _auditlogs;

        public UnitOfWork(CrmDbContext context)
        {
            _context = context;
        }

        public IRepository<User> Users
            => _users ??= new Repository<User>(_context);

        public IRepository<Role> Roles
            => _roles ??= new Repository<Role>(_context);

        public IRepository<Client> Clients
            => _clients ??= new Repository<Client>(_context);

        public IRepository<Product> Products
            => _products ??= new Repository<Product>(_context);

        public IRepository<Order> Orders
            => _orders ??= new Repository<Order>(_context);

        public IRepository<OrderProduct> OrderProducts
            => _orderProducts ??= new Repository<OrderProduct>(_context);

        public IRepository<Warehause> Warehauses
            => _warehauses ??= new Repository<Warehause>(_context);

        public IRepository<WarehauseProduct> WarehauseProducts
            => _warehauseProducts ??= new Repository<WarehauseProduct>(_context);

        public IRepository<Auditlog> Auditlogs
            => _auditlogs ??= new Repository<Auditlog>(_context);

        public int SaveChanges()
            => _context.SaveChanges();

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public async Task<int> ExecuteTransactionAsync(Func<Task<int>> operation)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var result = await operation();
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
